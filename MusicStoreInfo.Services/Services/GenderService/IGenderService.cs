using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.GenderService
{
    public interface IGenderService
    {
        Task CreateAsync(Gender model);
        Task DeleteAsync(int id);
        Task<Gender?> DetailsAsync(int id);
        Task EditAsync(int id, Gender model);
        Task<List<Gender>?> GetAllAsync();
        Task<Gender?> GetByIdAsync(int id);
    }
}