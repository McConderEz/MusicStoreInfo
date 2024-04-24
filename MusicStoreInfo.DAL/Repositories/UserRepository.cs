﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users.Include(u => u.Role)
                                         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _dbContext.Users.Include(u => u.Role)
                                         .FirstOrDefaultAsync(u => u.UserName.Equals(userName));
        }

        public async Task<List<User>> Get()
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .Include(u => u.Role)
                                   .ToListAsync();
        }
    }
}
