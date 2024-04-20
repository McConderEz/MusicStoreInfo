using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.MemberService
{
    public interface IMemberService
    {
        Task CreateAsync(Member model);
        Task DeleteAsync(int id);
        Task<Member?> DetailsAsync(int id);
        Task EditAsync(int id, Member model);
        Task<List<Member>?> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddGroupAsync(int id, int groupId);
        Task AddSpecializationAsync(int id, int specializationId);
        Task DeleteGroupAsync(int id, int groupId);
        Task DeleteSpecializationAsync(int id, int specializationId);
    }
}