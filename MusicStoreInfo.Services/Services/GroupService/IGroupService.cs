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
    }
}