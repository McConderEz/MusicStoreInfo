using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.GroupService
{
    public interface IGroupService
    {
        Task CreateAsync(Group model);
        Task DeleteAsync(int id);
        Task<Group?> DetailsAsync(int id);
        Task EditAsync(int id, Group model);
        Task<List<Group>?> GetAllAsync();
        Task<Group?> GetByIdAsync(int id);
        Task AddGenre(int id, int genreId);
        Task DeleteGenreAsync(int id, int genreId);
        Task AddMemberAsync(int id, int memberId);
        Task DeleteMemberAsync(int id, int memberId);
    }
}