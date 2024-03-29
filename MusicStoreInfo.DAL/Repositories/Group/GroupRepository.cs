﻿using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public GroupRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Group>> Get()
        {
            return await _dbContext.Groups
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<Group?> GetById(int id)
        {
            return await _dbContext.Groups
                .Include(a => a.Genres)
                .Include(a => a.Members)
                     .ThenInclude(a => a.Gender)
                .Include(a => a.Members)
                     .ThenInclude(a => a.Specializations)
                .Include(a => a.Albums)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Group group)
        {
            await _dbContext.AddAsync(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name, string image)
        {
            await _dbContext.Groups
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name)
                    .SetProperty(a => a.ImagePath, image));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Groups
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
