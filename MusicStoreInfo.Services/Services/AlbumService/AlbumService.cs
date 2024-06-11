using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _repository;

        public AlbumService(IAlbumRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Album model)
        {

            await _repository.Add(model);
        }

        public async Task<List<Album>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Album model)
        {
            var album = await _repository.GetById(id);

            if (album == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id,
                model.ListenerTypeId, model.GroupId, model.CompanyId,
                model.Name, model.Duration, model.ReleaseDate, model.SongsCount, model.ImagePath);
        }

        public async Task<Album?> DetailsAsync(int id)
        {
            var album = await _repository.GetById(id);

            if (album == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return album;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Album?> GetByIdAsync(int id)
        {
            var album = await _repository.GetById(id);
            return album;
        }

        public async Task AddStore(int id, Store store)
        {
            await _repository.AddStore(id, store);
        }
    }
}
