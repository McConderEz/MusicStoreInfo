using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        Task AddAsync(ShoppingCart shoppingCart);
        Task AddProductAsync(int id, int storeId, int albumId, int quantity);
        Task DeleteProductAsync(int id, int albumId, int storeId);
        Task<ShoppingCart?> GetByIdAsync(int id);
        Task<ShoppingCart?> GetByUserNameAsync(string userName);
    }
}