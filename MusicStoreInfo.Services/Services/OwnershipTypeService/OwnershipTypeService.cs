using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.OwnershipTypeService
{
    public class OwnershipTypeService : IOwnershipTypeService
    {
        private readonly IOwnershipTypeRepository _repository;

        public OwnershipTypeService(IOwnershipTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(OwnershipType model)
        {
            await _repository.Add(model);
        }

        public async Task<List<OwnershipType>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, OwnershipType model)
        {
            var ownershipType = _repository.GetById(id);

            if (ownershipType == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<OwnershipType?> DetailsAsync(int id)
        {
            var ownershipType = await _repository.GetById(id);

            if (ownershipType == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return ownershipType;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<OwnershipType?> GetByIdAsync(int id)
        {
            var ownershipType = await _repository.GetById(id);
            return ownershipType;
        }
    }
}
