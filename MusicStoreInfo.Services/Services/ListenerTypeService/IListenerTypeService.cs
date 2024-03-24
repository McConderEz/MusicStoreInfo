using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.ListenerTypeService
{
    public interface IListenerTypeService
    {
        Task CreateAsync(ListenerType model);
        Task DeleteAsync(int id);
        Task<ListenerType?> DetailsAsync(int id);
        Task EditAsync(int id, ListenerType model);
        Task<List<ListenerType>?> GetAllAsync();
        Task<ListenerType?> GetByIdAsync(int id);
    }
}