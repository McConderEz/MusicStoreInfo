using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public GenderRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Gender>> Get()
        {
            return await _dbContext.Genders
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Members)
                .ToListAsync();
        }

        public async Task<Gender?> GetById(int id)
        {
            return await _dbContext.Genders
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(string name)
        {
            var gender = new Gender
            {
                Name = name
            };

            await _dbContext.AddAsync(gender);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.Genders
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Genders
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
