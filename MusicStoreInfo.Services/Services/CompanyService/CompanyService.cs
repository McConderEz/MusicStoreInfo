using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.CompanySerivce
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Company model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Company>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Company model)
        {
            var company = _repository.GetById(id);

            if (company == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.DistrictId, model.PhoneNumber, model.Name);
        }

        public async Task<Company?> DetailsAsync(int id)
        {
            var company = await _repository.GetById(id);

            if (company == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return company;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            var company = await _repository.GetById(id);
            return company;
        }
    }
}
