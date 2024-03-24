using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.SpecializationService
{
    public interface ISpecializationService
    {
        Task CreateAsync(Specialization model);
        Task DeleteAsync(int id);
        Task<Specialization?> DetailsAsync(int id);
        Task EditAsync(int id, Specialization model);
        Task<List<Specialization>?> GetAllAsync();
        Task<Specialization?> GetByIdAsync(int id);
    }
}