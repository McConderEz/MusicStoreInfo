using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IShoppingCartRepository
    {
        Task Add(ShoppingCart shoppingCart);
        Task AddProduct(int id, int storeId, int albumId, int quantity);
        Task DeleteProductAsync(int id, int productId);
        Task<ShoppingCart?> GetById(int id);
        Task<ShoppingCart?> GetByUserName(string userName);
    }
}