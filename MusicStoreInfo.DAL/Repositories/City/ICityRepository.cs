using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface ICityRepository
    {
        Task Add(City city);
        Task Delete(int id);
        Task<List<City>> Get();
        Task<City?> GetById(int id);
        Task Update(int id, string name);
    }
}