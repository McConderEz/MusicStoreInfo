using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IReviewRepository
    {
        Task Add(Review review);
        Task Delete(int id);
        Task<List<Review>> Get();
        Task<Review?> GetById(int id);
        Task Update(int id, string name, string image);
    }
}