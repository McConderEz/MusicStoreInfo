using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public UserRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _dbContext.Users.Include(u => u.Role)
                                         .Include(u => u.ShoppingCart)
                                         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserName(string userName)
        {
            return await _dbContext.Users.Include(u => u.Role)
                                         .Include(u => u.Store)  
                                         .Include(u => u.ShoppingCart)
                                         .FirstOrDefaultAsync(u => u.UserName.Equals(userName));
        }

        public async Task<List<User>> Get()
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .Include(u => u.Role)
                                   .Include(u => u.Store)
                                   .ToListAsync();
        }
        public async Task Update(int id, string userName, string passwordHash, string? email, string? phoneNumber, string? imagePath,int roleId, int? storeId, bool isBlocked)
        {
            await _dbContext.Users
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.UserName, userName)
                    .SetProperty(a => a.PasswordHash, passwordHash)
                    .SetProperty(a => a.Email, email)
                    .SetProperty(a => a.PhoneNumber, phoneNumber)
                    .SetProperty(a => a.ImagePath, imagePath)
                    .SetProperty(a => a.RoleId, roleId)
                    .SetProperty(a => a.StoreId, storeId)
                    .SetProperty(a => a.IsBlocked, isBlocked));
            await _dbContext.SaveChangesAsync();
        }
    }
}
