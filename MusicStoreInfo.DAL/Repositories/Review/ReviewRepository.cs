using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public ReviewRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Review>> Get()
        {
            return await _dbContext.Reviews
                .AsNoTracking()
                .OrderBy(r => r.TimeCreated)
                .Include(r => r.Product)
                .Include (r => r.User)
                .ToListAsync();
        }

        public async Task<Review?> GetById(int id)
        {
            return await _dbContext.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Add(Review review)
        {
            await _dbContext.AddAsync(review);
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
