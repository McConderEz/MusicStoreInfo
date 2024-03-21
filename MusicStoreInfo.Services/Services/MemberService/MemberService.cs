using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.MemberService
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Member model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Member>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Member model)
        {
            var member = _repository.GetById(id);

            if (member == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name, model.SecondName, model.Age, model.GenderId);
        }

        public async Task<Member?> DetailsAsync(int id)
        {
            var member = await _repository.GetById(id);

            if (member == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return member;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }
    }
}
