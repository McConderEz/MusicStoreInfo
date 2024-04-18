using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGenreRepository
    {
        Task Add(Genre genre);
        Task Delete(int id);
        Task<List<Genre>> Get();
        Task<Genre?> GetById(int id);
        Task Update(int id, string name);
        Task AddGroup(int id, int groupId);
        Task DeleteGroup(int id, int groupId);
    }
}