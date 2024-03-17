using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface ISpecializationRepository
    {
        Task Add(string name);
        Task Delete(int id);
        Task<List<Specialization>> Get();
        Task<Specialization?> GetById(int id);
        Task Update(int id, string name);
    }
}