using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGroupRepository
    {
        Task Add(string name);
        Task Delete(int id);
        Task<List<Group>> Get();
        Task<Group?> GetById(int id);
        Task Update(int id, string name);
    }
}