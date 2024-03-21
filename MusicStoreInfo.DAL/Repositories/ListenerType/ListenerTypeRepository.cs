using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class ListenerTypeRepository : IListenerTypeRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public ListenerTypeRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListenerType>> Get()
        {
            return await _dbContext.ListenerTypes
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Albums)
                .ToListAsync();
        }

        public async Task<ListenerType?> GetById(int id)
        {
            return await _dbContext.ListenerTypes
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(ListenerType listenerType)
        {
            await _dbContext.AddAsync(listenerType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.ListenerTypes
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.ListenerTypes
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
