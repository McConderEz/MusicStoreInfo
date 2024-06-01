using Microsoft.Data.SqlClient;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Infrastructure;
using MusicStoreInfo.Services.Services.ShoppingCartService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MusicStoreInfo.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IJwtProvider _jwtProvider;
        private readonly MusicStoreDbContext _dbContext;

        public AccountService(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IShoppingCartService shoppingCartService,
            MusicStoreDbContext dbContext)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _shoppingCartService = shoppingCartService;
            _dbContext = dbContext;
        }

        public async Task Register(string userName, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var user = await _userRepository.GetByUserName(userName);

            string createUserSql = $@"
                                 CREATE LOGIN {userName} WITH PASSWORD = '{hashedPassword}';
                                 CREATE USER {userName} FOR LOGIN {userName};";

            if (user == null)
            {
                using (SqlConnection connection = new SqlConnection(MusicStoreDbContext.CONNECTION_STRING))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(createUserSql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    string getPrincipalIdSql = $@"
                                                  SELECT principal_id 
                                                  FROM sys.server_principals 
                                                  WHERE name = @userName";

                    AssignRoleToUser(MusicStoreDbContext.CONNECTION_STRING, userName, "Admin");

                    using (SqlCommand command = new SqlCommand(getPrincipalIdSql, connection))
                    {
                        command.Parameters.AddWithValue("@userName", userName);
                        var principalId = (int)command.ExecuteScalar();
                        user = new User
                        {
                            UserName = userName,
                            PasswordHash = hashedPassword,
                            RoleId = 1,
                            PrincipalId = principalId
                        };

                        await _userRepository.Add(user);
                        ShoppingCart shoppingCart = new ShoppingCart { UserId = user.Id };
                        await _shoppingCartService.AddAsync(shoppingCart);
                        user.ShoppingCartId = shoppingCart.Id;
                        user.ShoppingCart = shoppingCart;
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public void AssignRoleToUser(string connectionString, string userName, string roleName)
        {
            string assignRoleSql = $@"
                         ALTER ROLE [{roleName}] ADD MEMBER [{userName}];
                                                                        ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(assignRoleSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"User '{userName}' assigned to role '{roleName}' successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public async Task<(string, User)> Login(string userName, string password)
        {
            var user = await _userRepository.GetByUserName(userName);
            
            //TODO: Добавить уведомление, что неверные данные 

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if(result == false)
            {
                return (null, null);
            }

            var token = _jwtProvider.GenerateToken(user);

            return (token, user);
        }

        public async Task<List<User>> GetAsync()
        {
            return await _userRepository.Get();
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _userRepository.GetByUserName(userName);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task EditAsync(int id, User model)
        {
            await _userRepository.Update(id, model.UserName, model.PasswordHash,
                model.Email, model.PhoneNumber, model.ImagePath, model.RoleId, model.StoreId, model.IsBlocked);
        }
    }
}
