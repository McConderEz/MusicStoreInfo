using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public OrderRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> Get()
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .OrderBy(o => o.OrderDate)
                .Include(o => o.User)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Store)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Album)
                .ToListAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Store)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Album)
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Add(Order order)
        {
            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name, string image)
        {
            //TODO:реализовать
        }

        public async Task Delete(int id)
        {
            //TODO:реализовать
        }
    }
}
