using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IStoreRepository
    {
        Task Add(Store store);
        Task Delete(int id);
        Task<List<Store>> Get();
        Task<Store?> GetById(int id);
        Task Update(int id, int ownershipId, int districtId, string phoneNumber, string name, DateTime yearOpened);
    }
}