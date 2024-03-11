using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface ICompanyRepository
    {
        Task Add(int districtId, string phoneNumber, string name);
        Task Delete(int id);
        Task<List<Company>> Get();
        Task<Company?> GetById(int id);
        Task Update(int id, int districtId, string phoneNumber, string name);
    }
}