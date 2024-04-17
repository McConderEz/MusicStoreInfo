using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGroupRepository
    {
        Task Add(Group group);
        Task Delete(int id);
        Task<List<Group>> Get();
        Task<Group?> GetById(int id);
        Task Update(int id, string name, string image);
        Task AddGenre(int id, int genreId);
        Task DeleteGenre(int id, int genreId);
        Task AddMember(int id, int memberId);
        Task DeleteMember(int id, int memberId);
    }
}