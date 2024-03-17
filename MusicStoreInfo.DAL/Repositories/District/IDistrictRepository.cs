using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IDistrictRepository
    {
        Task Add(int cityId, string name);
        Task Delete(int id);
        Task<List<District>> Get();
        Task<District?> GetById(int id);
        Task Update(int id, int cityId, string name);
    }
}