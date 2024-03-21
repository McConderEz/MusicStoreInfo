using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.GenreService
{
    public interface IGenreService
    {
        Task CreateAsync(Genre model);
        Task DeleteAsync(int id);
        Task<Genre?> DetailsAsync(int id);
        Task EditAsync(int id, Genre model);
        Task<List<Genre>?> GetAllAsync();
    }
}