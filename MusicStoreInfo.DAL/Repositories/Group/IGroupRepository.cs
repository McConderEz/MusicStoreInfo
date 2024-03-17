using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGroupRepository
    {
        Task Add(string name, byte[] image);
        Task Delete(int id);
        Task<List<Group>> Get();
        Task<Group?> GetById(int id);
        Task Update(int id, string name, byte[] image);
    }
}