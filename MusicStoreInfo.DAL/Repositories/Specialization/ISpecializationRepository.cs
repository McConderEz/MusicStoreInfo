using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface ISpecializationRepository
    {
        Task Add(Specialization specialization);
        Task Delete(int id);
        Task<List<Specialization>> Get();
        Task<Specialization?> GetById(int id);
        Task Update(int id, string name);
        Task AddMember(int id, int memberId);
        Task DeleteMember(int id, int memberId);
    }
}