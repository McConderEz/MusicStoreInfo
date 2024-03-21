using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.GenderService
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _repository;

        public GenderService(IGenderRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Gender model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Gender>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Gender model)
        {
            var gender = _repository.GetById(id);

            if (gender == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<Gender?> DetailsAsync(int id)
        {
            var gender = await _repository.GetById(id);

            if (gender == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return gender;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }
    }
}
