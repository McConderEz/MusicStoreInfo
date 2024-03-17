using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IListenerTypeRepository
    {
        Task Add(string name);
        Task Delete(int id);
        Task<List<ListenerType>> Get();
        Task<ListenerType?> GetById(int id);
        Task Update(int id, string name);
    }
}