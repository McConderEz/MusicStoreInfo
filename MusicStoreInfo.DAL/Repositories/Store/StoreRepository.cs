using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public StoreRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Store>> Get()
        {
            return await _dbContext.Stores
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Albums)
                .Include(a => a.Products)
                .Include(a => a.OwnershipType)
                .Include(a => a.District)
                .ToListAsync();
        }

        public async Task<Store?> GetById(int id)
        {
            return await _dbContext.Stores
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(int ownershipId, int districtId, string phoneNumber,
            string name, DateTime yearOpened)
        {
            var store = new Store
            {
                OwnershipTypeId = ownershipId,
                DistrictId = districtId,
                PhoneNumber = phoneNumber,
                Name = name,
                YearOpened = yearOpened
            };

            await _dbContext.AddAsync(store);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int ownershipId, int districtId, string phoneNumber,
            string name, DateTime yearOpened)
        {
            await _dbContext.Stores
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.OwnershipTypeId, ownershipId)
                    .SetProperty(a => a.DistrictId, districtId)
                    .SetProperty(a => a.PhoneNumber, phoneNumber)
                    .SetProperty(a => a.Name, name)
                    .SetProperty(a => a.YearOpened, yearOpened));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Stores
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
