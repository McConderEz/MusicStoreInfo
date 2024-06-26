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
                .Include(g => g.Genres)
                .ToListAsync();
        }

        public async Task<Group?> GetById(int id)
        {
            return await _dbContext.Groups
                .Include(a => a.Albums)
                    .ThenInclude(a => a.ListenerType)
                .Include(a => a.Genres)
                .Include(a => a.Members)
                     .ThenInclude(a => a.Gender)
                .Include(a => a.Members)
                     .ThenInclude(a => a.Specializations)
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

        public async Task AddGenre(int id, int genreId)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            var genre = await _dbContext.Genres.FindAsync(genreId);

            if (group != null && genre != null)
            {
                group.Genres.Add(genre);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteGenre(int id, int genreId)
        {
            var group = await _dbContext.Groups.Include(g => g.Genres).FirstOrDefaultAsync(g => g.Id == id);
            var genre = group.Genres.FirstOrDefault(g => g.Id == genreId);
            if(group != null && genre != null)
            {
                group.Genres.Remove(genre);
                await _dbContext.SaveChangesAsync();    
            }
        }

        public async Task AddMember(int id, int memberId)
        {
            var group = await _dbContext.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == id);
            var member = await _dbContext.Members.FindAsync(memberId);

            if (group != null && member != null)
            {
                group.Members.Add(member);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMember(int id, int memberId)
        {
            var group = await _dbContext.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == id);
            var member = group.Members.FirstOrDefault(g => g.Id == memberId);

            if (group != null && member != null)
            {
                group.Members.Remove(member);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
