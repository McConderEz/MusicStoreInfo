using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        Task AddAsync(ShoppingCart shoppingCart);
        Task AddProductAsync(int id, int productId);
        Task DeleteProductAsync(int id, int productId);
        Task<ShoppingCart?> GetByIdAsync(int id);
        Task<ShoppingCart?> GetByUserNameAsync(string userName);
    }
}