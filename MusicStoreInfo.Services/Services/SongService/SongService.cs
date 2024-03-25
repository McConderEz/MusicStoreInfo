using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.SongService
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repository;

        public SongService(ISongRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Song model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Song>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Song model)
        {
            var song = _repository.GetById(id);

            if (song == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.AlbumId, model.Name, model.Duration);
        }

        public async Task<Song?> DetailsAsync(int id)
        {
            var song = await _repository.GetById(id);

            if (song == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return song;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            var song = await _repository.GetById(id);
            return song;
        }
    }
}
