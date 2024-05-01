using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ShoppingCartService
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart?> GetByIdAsync(int id)
        {
            return await _shoppingCartRepository.GetById(id);
        }

        public async Task<ShoppingCart?> GetByUserNameAsync(string userName)
        {
            return await _shoppingCartRepository.GetByUserName(userName);
        }

        public async Task AddAsync(ShoppingCart shoppingCart)
        {
            await _shoppingCartRepository.Add(shoppingCart);
        }

        public async Task AddProductAsync(int id, int storeId, int albumId)
        {
            await _shoppingCartRepository.AddProduct(id, storeId,  albumId);
        }

        public async Task DeleteProductAsync(int id, int storeId, int albumId)
        {
            await _shoppingCartRepository.DeleteProduct(id, storeId, albumId);
        }
    }
}
