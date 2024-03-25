using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.CompanySerivce
{
    public interface ICompanyService
    {
        Task CreateAsync(Company model);
        Task DeleteAsync(int id);
        Task<Company?> DetailsAsync(int id);
        Task EditAsync(int id, Company model);
        Task<List<Company>?> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
    }
}