using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.SpecializationService
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _repository;

        public SpecializationService(ISpecializationRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Specialization model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Specialization>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Specialization model)
        {
            var specialization = _repository.GetById(id);

            if (specialization == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<Specialization?> DetailsAsync(int id)
        {
            var specialization = await _repository.GetById(id);

            if (specialization == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return specialization;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Specialization?> GetByIdAsync(int id)
        {
            var specialization = await _repository.GetById(id);
            return specialization;
        }
    }
}
