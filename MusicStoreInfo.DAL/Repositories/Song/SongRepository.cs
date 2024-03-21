using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public SongRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Song>> Get()
        {
            return await _dbContext.Songs
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Album)
                .ToListAsync();
        }

        public async Task<Song?> GetById(int id)
        {
            return await _dbContext.Songs
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Song song)
        {
            await _dbContext.AddAsync(song);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int albumId, string name, int duration)
        {
            await _dbContext.Songs
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.AlbumId, albumId)
                    .SetProperty(a => a.Name, name)
                    .SetProperty(a => a.Duration, duration));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Songs
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
