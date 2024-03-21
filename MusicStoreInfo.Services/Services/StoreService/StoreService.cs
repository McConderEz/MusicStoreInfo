using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.StoreService
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;

        public StoreService(IStoreRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Store model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Store>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Store model)
        {
            var store = _repository.GetById(id);

            if (store == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.OwnershipTypeId, model.DistrictId, model.PhoneNumber, model.Name, model.YearOpened);
        }

        public async Task<Store?> DetailsAsync(int id)
        {
            var store = await _repository.GetById(id);

            if (store == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return store;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }
    }
}
