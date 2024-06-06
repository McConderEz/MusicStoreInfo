using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.ReviewService
{
    public interface IReviewService
    {
        Task Add(Review review);
        Task Delete(int id);
        Task<List<Review>> GetAsync();
        Task<Review?> GetByIdAsync(int id);
        Task Update(int id, string name, string image);
    }
}