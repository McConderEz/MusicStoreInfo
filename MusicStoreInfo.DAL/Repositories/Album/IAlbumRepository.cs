using Microsoft.AspNetCore.Http;
using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IAlbumRepository
    {
        //Task Add(int listenerTypeId, int companyId, int groupId, string name, int duration, DateTime releaseDate, int songsCount, byte[] image);
        Task Add(Album album);
        Task Delete(int id);
        Task<List<Album>> Get();
        Task<Album?> GetById(int id);
        Task Update(int id, int listenerTypeId, int companyId, int groupId, string name, int duration, DateTime releaseDate, int songsCount, byte[] image);        
    }
}