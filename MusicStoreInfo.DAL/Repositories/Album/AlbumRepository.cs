using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public AlbumRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Album>> Get()
        {
            return await _dbContext.Albums
                .AsNoTracking()
                .Include(a => a.Group)
                    .ThenInclude(g => g.Genres)
                .Include(a => a.ListenerType)
                .Include(a => a.Songs)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<List<Album>> GetByPage(int page,int pageSize)
        {
            return await _dbContext.Albums
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Album?> GetById(int id)
        {
            return await _dbContext.Albums
                .AsNoTracking()
                .Include(a => a.Songs)
                .Include(a => a.Stores)
                .Include(a => a.Company)
                .Include(a => a.Products)
                    .ThenInclude(p => p.Store)
                        .ThenInclude(s => s.District)
                            .ThenInclude(d => d.City)
                .Include(a => a.ListenerType)
                .Include(a => a.Group)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Album album)
        {
            await _dbContext.AddAsync(album);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int listenerTypeId, int companyId, int groupId,
            string name, int duration, DateTime releaseDate, int songsCount, string imagePath)
        {
            await _dbContext.Albums
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.ListenerTypeId, listenerTypeId)
                    .SetProperty(a => a.CompanyId, companyId)
                    .SetProperty(a => a.GroupId, groupId)
                    .SetProperty(a => a.Name, name)
                    .SetProperty(a => a.Duration, duration)
                    .SetProperty(a => a.ReleaseDate, releaseDate)
                    .SetProperty(a => a.SongsCount, songsCount)
                    .SetProperty(a => a.ImagePath, imagePath));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Albums
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddStore(int id, Store store)
        {
           var album = await _dbContext.Albums.FindAsync(id); 

           if(album != null)
           {
                album.Stores.Add(store);
                await _dbContext.SaveChangesAsync();
           }
        }
    }
}
