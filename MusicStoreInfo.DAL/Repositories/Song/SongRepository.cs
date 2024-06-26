﻿using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
                    .ThenInclude(a => a.Group)
                .ToListAsync();
        }

        public async Task<Song?> GetById(int id)
        {
            return await _dbContext.Songs.Include(s => s.Album)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Song song)
        {
            var album = await _dbContext.Albums.FindAsync(song.AlbumId);

            if(album != null)
            {
                album.SongsCount++;
                album.Duration += song.Duration;
            }

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
            var song = await _dbContext.Songs.FindAsync(id);
            var album = song != null ? await _dbContext.Albums.FindAsync(song.AlbumId) : null;

            if(album != null)
            {
                album.SongsCount--;
                album.Duration -= song.Duration;
            }
            else
            {
                return;
            }

            await _dbContext.Songs
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
