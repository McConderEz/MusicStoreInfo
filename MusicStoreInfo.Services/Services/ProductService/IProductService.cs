using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.ProductService
{
    public interface IProductService
    {
        Task CreateAsync(Product model);
        Task DeleteAsync(int id);
        Task<Product?> DetailsAsync(int id);
        Task EditAsync(int id, Product model);
        Task<List<Product>?> GetAllAsync();
        Task AddReviewAsync(Review review, int id);
    }
}