using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGenreRepository
    {
        Task Add(string name);
        Task Delete(int id);
        Task<List<Genre>> Get();
        Task<Genre?> GetById(int id);
        Task Update(int id, string name);
    }
}