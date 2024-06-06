using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public ProductRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> Get()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Album)
                    .ThenInclude(a => a.Group)
                        .ThenInclude(g => g.Genres)
                .Include(a => a.Store)
                    .ThenInclude(s => s.District)
                        .ThenInclude(d => d.City)
                .Include(a => a.Reviews)
                .ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(a => a.Album)
                    .ThenInclude(a => a.Group)
                .Include(a => a.Album)
                    .ThenInclude(a => a.Songs)
                .Include(a => a.Store)
                .Include(a => a.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Product product)
        {
            var item = await _dbContext.Products.FindAsync(product.AlbumId, product.StoreId);

            if(item != null)
            {
                item.Quantity += product.Quantity;
                item.Price = product.Price;
                await _dbContext.SaveChangesAsync();
                return;
            }

            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int albumId, int storeId, decimal price,
            int quantity, DateTime dateReceived)
        {
            await _dbContext.Products
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.AlbumId, albumId)
                    .SetProperty(a => a.StoreId, storeId)
                    .SetProperty(a => a.Price, price)
                    .SetProperty(a => a.Quantity, quantity)
                    .SetProperty(a => a.DateReceived, dateReceived));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Products
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddReview(Review review, int id)
        {
            var product = await GetById(id);

            if(product == null)
            {
                product.Reviews.Add(review);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
