using Microsoft.AspNetCore.Http;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.CityService
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repository;

        public CityService(ICityRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(City model)
        {
            await _repository.Add(model);
        }

        public async Task<List<City>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, City model)
        {
            var album = _repository.GetById(id);

            if (album == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<City?> DetailsAsync(int id)
        {
            var city = await _repository.GetById(id);

            if (city == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return city;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

    }
}
