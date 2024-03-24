using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.OwnershipTypeService
{
    public interface IOwnershipTypeService
    {
        Task CreateAsync(OwnershipType model);
        Task DeleteAsync(int id);
        Task<OwnershipType?> DetailsAsync(int id);
        Task EditAsync(int id, OwnershipType model);
        Task<List<OwnershipType>?> GetAllAsync();
        Task<OwnershipType?> GetByIdAsync(int id);
    }
}