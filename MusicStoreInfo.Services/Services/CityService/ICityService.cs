using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.CityService
{
    public interface ICityService
    {
        Task CreateAsync(City model);
        Task DeleteAsync(int id);
        Task<City?> DetailsAsync(int id);
        Task EditAsync(int id, City model);
        Task<List<City>?> GetAllAsync();
    }
}