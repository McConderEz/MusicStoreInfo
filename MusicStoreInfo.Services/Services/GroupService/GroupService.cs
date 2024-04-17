using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;

        public GroupService(IGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Group model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Group>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Group model)
        {
            var group = _repository.GetById(id);

            if (group == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name, model.ImagePath);
        }

        public async Task<Group?> DetailsAsync(int id)
        {
            var group = await _repository.GetById(id);

            if (group == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return group;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            var group = await _repository.GetById(id);
            return group;
        }

        public async Task AddGenre(int id, int genreId)
        {
            await _repository.AddGenre(id, genreId);
        }

        public async Task DeleteGenreAsync(int id, int genreId)
        {
            await _repository.DeleteGenre(id, genreId);
        }

        public async Task AddMemberAsync(int id, int memberId)
        {
            await _repository.AddMember(id, memberId);
        }

        public async Task DeleteMemberAsync(int id, int memberId)
        {
            await _repository.DeleteMember(id, memberId);
        }
    }
}
