using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using MusicStoreInfo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AccountService(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task Register(string userName, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var user = await _userRepository.GetByUserName(userName);

            if(user == null)
            {
                user = new User
                {
                    UserName = userName,
                    PasswordHash = hashedPassword,
                    RoleId = 2
                };

                await _userRepository.Add(user);
            }
        }

        public async Task<(string, User)> Login(string userName, string password)
        {
            var user = await _userRepository.GetByUserName(userName);

            //TODO: Добавить уведомление, что неверные данные 

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            return (token, user);
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
                model.Email, model.PhoneNumber, model.ImagePath);
        }
    }
}
