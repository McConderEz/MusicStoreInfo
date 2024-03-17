using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public DistrictRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<District>> Get()
        {
            return await _dbContext.Districts
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Companies)
                .Include(a => a.City)
                .Include(a => a.Stores)
                .ToListAsync();
        }

        public async Task<District?> GetById(int id)
        {
            return await _dbContext.Districts
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(int cityId, string name)
        {
            var district = new District
            {
                CityId = cityId,
                Name = name
            };

            await _dbContext.AddAsync(district);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int cityId, string name)
        {
            await _dbContext.Districts
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.CityId, cityId)
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Districts
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
