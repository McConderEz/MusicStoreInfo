using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public GenreRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Genre>> Get()
        {
            return await _dbContext.Genres
                .AsNoTracking()
                .OrderBy(a => a.Id)                
                .ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await _dbContext.Genres
                .Include(a => a.Groups)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Genre genre)
        {

            await _dbContext.AddAsync(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.Genres
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Genres
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddGroup(int id, int groupId)
        {
            var genre = await _dbContext.Genres.Include(g => g.Groups).FirstOrDefaultAsync(g => g.Id == id);
            var group = await _dbContext.Groups.FindAsync(groupId);

            if (group != null && genre != null)
            {
                genre.Groups.Add(group);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteGroup(int id, int groupId)
        {
            var genre = await _dbContext.Genres.Include(g => g.Groups).FirstOrDefaultAsync(g => g.Id == id);
            var group = await _dbContext.Groups.FindAsync(groupId);

            if (group != null && genre != null)
            {
                genre.Groups.Remove(group);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
