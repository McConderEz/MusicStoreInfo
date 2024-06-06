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
                .Include(scp => scp.ShoppingCartProducts)
                    .ThenInclude(scp => scp.Product)
                        .ThenInclude(p => p.Album)
                .Include(scp => scp.ShoppingCartProducts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Store)
                            .ThenInclude(s => s.District)
                             .ThenInclude(d => d.City)
                .Include(s => s.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ShoppingCart?> GetByUserName(string userName)
        {
            return await _dbContext.ShoppingCarts
                .Include(scp => scp.ShoppingCartProducts)
                    .ThenInclude(scp => scp.Product)
                        .ThenInclude(p => p.Album)
                .Include(scp => scp.ShoppingCartProducts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Album)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.UserName == userName);
        }

        public async Task Add(ShoppingCart shoppingCart)
        {
            await _dbContext.AddAsync(shoppingCart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddProduct(int id, int storeId, int albumId, int quantity)
        {
            try
            {
                var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(g => g.Id == id);

                var product = await _dbContext.Products.FindAsync(storeId, albumId);

                if(quantity <= 0 && product != null)
                {
                    DeleteProduct(id, product!.Id);
                }

                var shoppingCartProduct = await _dbContext.ShoppingCartProductLinks.FirstOrDefaultAsync(scp => scp.ProductId == product.Id && scp.ShoppingCartId == shoppingCart.Id);

                if (shoppingCartProduct != null)
                {
                    shoppingCartProduct.Quantity = quantity;
                }
                else if (shoppingCart != null && product != null)
                {
                    shoppingCartProduct = new ShoppingCartProductLink
                    {
                        ShoppingCartId = shoppingCart.Id,
                        ProductId = product.Id,
                        Quantity = quantity,
                        Product = product,
                        ShoppingCart = shoppingCart
                    };

                    _dbContext.ShoppingCartProductLinks.Add(shoppingCartProduct);
                    shoppingCart.ShoppingCartProducts.Add(shoppingCartProduct);

                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteProduct(int id, int productId)
        {
            try
            {
                var shoppingCart = await _dbContext.ShoppingCarts.Include(s => s.ShoppingCartProducts).ThenInclude(scp => scp.Product).FirstOrDefaultAsync(s => s.Id == id);
                var product = shoppingCart.ShoppingCartProducts.FirstOrDefault(p => p.ProductId == productId && p.ShoppingCartId == id);
                if (shoppingCart != null && product != null)
                {
                    shoppingCart.ShoppingCartProducts.Remove(product);
                    await _dbContext.SaveChangesAsync();
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
