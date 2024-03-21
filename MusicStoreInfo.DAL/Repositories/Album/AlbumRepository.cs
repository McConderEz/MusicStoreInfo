﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
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
                .OrderBy(a => a.Id)
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
            string name, int duration, DateTime releaseDate, int songsCount, byte[] image)
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
                    .SetProperty(a => a.Image, image));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Albums
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
       
    }
}