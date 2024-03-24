using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.DistrictService
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _repository;

        public DistrictService(IDistrictRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(District model)
        {
            await _repository.Add(model);
        }

        public async Task<List<District>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, District model)
        {
            var district = _repository.GetById(id);

            if (district == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.CityId, model.Name);
        }

        public async Task<District?> DetailsAsync(int id)
        {
            var district = await _repository.GetById(id);

            if (district == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return district;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<District?> GetByIdAsync(int id)
        {
            var district = await _repository.GetById(id);
            return district;
        }
    }
}
