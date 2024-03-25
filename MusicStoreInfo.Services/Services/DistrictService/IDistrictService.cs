using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.DistrictService
{
    public interface IDistrictService
    {
        Task CreateAsync(District model);
        Task DeleteAsync(int id);
        Task<District?> DetailsAsync(int id);
        Task EditAsync(int id, District model);
        Task<List<District>?> GetAllAsync();
        Task<District?> GetByIdAsync(int id);
    }
}