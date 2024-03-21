using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.StoreService
{
    public interface IStoreService
    {
        Task CreateAsync(Store model);
        Task DeleteAsync(int id);
        Task<Store?> DetailsAsync(int id);
        Task EditAsync(int id, Store model);
        Task<List<Store>?> GetAllAsync();
    }
}