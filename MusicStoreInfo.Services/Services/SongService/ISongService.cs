using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.SongService
{
    public interface ISongService
    {
        Task CreateAsync(Song model);
        Task DeleteAsync(int id);
        Task<Song?> DetailsAsync(int id);
        Task EditAsync(int id, Song model);
        Task<List<Song>?> GetAllAsync();
        Task<Song?> GetByIdAsync(int id);
    }
}