using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface ISongRepository
    {
        Task Add(Song song);
        Task Delete(int id);
        Task<List<Song>> Get();
        Task<Song?> GetById(int id);
        Task Update(int id, int albumId, string name, int duration);
    }
}