using Microsoft.AspNetCore.Http;
using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.AlbumService
{
    public interface IAlbumService
    {
        Task CreateAsync(Album model);
        Task DeleteAsync(int id);
        Task<Album?> DetailsAsync(int id);
        Task EditAsync(int id, Album model);
        Task<List<Album>?> GetAllAsync();
        Task<Album?> GetByIdAsync(int id);
        Task AddStore(int id, Store store);
    }
}