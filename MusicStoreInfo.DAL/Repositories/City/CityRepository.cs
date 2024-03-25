using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public CityRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<City>> Get()
        {
            return await _dbContext.Cities
                .AsNoTracking()
                .OrderBy(a => a.Id)                
                .ToListAsync();
        }

        public async Task<City?> GetById(int id)
        {
            return await _dbContext.Cities
                .Include(a => a.Districts)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(City city)
        {
            await _dbContext.AddAsync(city);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.Cities
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Cities
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
