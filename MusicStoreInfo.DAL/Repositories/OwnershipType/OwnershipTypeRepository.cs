using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class OwnershipTypeRepository : IOwnershipTypeRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public OwnershipTypeRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OwnershipType>> Get()
        {
            return await _dbContext.OwnershipTypes
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Stores)
                .ToListAsync();
        }

        public async Task<OwnershipType?> GetById(int id)
        {
            return await _dbContext.OwnershipTypes
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(string name)
        {
            var ownershipType = new OwnershipType
            {
                Name = name
            };

            await _dbContext.AddAsync(ownershipType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.OwnershipTypes
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.OwnershipTypes
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
