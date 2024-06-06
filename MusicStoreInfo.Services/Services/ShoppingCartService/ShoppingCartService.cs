using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ShoppingCartService
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly MusicStoreDbContext _dbContext;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, MusicStoreDbContext dbContext)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _dbContext = dbContext;
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

        public async Task AddProductAsync(int id, int storeId, int albumId, int quantity)
        {
            await _shoppingCartRepository.AddProduct(id, storeId,  albumId, quantity);
        }

        public async Task DeleteProductAsync(int id, int albumId, int storeId)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.StoreId == storeId && p.AlbumId == albumId);
                await _shoppingCartRepository.DeleteProduct(id, product.Id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
