using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public ShoppingCartRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ShoppingCart?> GetById(int id)
        {
            return await _dbContext.ShoppingCarts
                .Include(s => s.Products)
                    .ThenInclude(p => p.Album)
                .Include(s => s.Products)
                    .ThenInclude(p => p.Store)
                .Include(s => s.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ShoppingCart?> GetByUserName(string userName)
        {
            return await _dbContext.ShoppingCarts
                .Include(s => s.Products)
                    .ThenInclude(p => p.Album)
                .Include(s => s.Products)
                    .ThenInclude(p => p.Store)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.UserName == userName);
        }

        public async Task Add(ShoppingCart shoppingCart)
        {
            await _dbContext.AddAsync(shoppingCart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddProduct(int id, int storeId, int albumId)
        {
            var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(g => g.Id == id);
            var product = await _dbContext.Products.FindAsync(storeId, albumId);

            if (shoppingCart.Products.FirstOrDefault(product) != null)
                return;

            if (shoppingCart != null && product != null)
            {
                shoppingCart.Products.Add(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(int id, int storeId, int albumId)
        {
            var shoppingCart = await _dbContext.ShoppingCarts.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == id);
            var product = shoppingCart.Products.FirstOrDefault(p => p.StoreId == storeId && p.AlbumId == albumId);
            if (shoppingCart != null && product != null)
            {
                shoppingCart.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
