using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ListenerTypeService
{
    public class ListenerTypeService : IListenerTypeService
    {
        private readonly IListenerTypeRepository _repository;

        public ListenerTypeService(IListenerTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(ListenerType model)
        {
            await _repository.Add(model);
        }

        public async Task<List<ListenerType>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, ListenerType model)
        {
            var listenerType = _repository.GetById(id);

            if (listenerType == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<ListenerType?> DetailsAsync(int id)
        {
            var listenerType = await _repository.GetById(id);

            if (listenerType == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return listenerType;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }
    }
}
