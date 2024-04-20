using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public SpecializationRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Specialization>> Get()
        {
            return await _dbContext.Specializations
                .AsNoTracking()
                .OrderBy(a => a.Id)                
                .ToListAsync();
        }

        public async Task<Specialization?> GetById(int id)
        {
            return await _dbContext.Specializations
                .Include(a => a.Members)
                    .ThenInclude(m => m.Gender)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Specialization specialization)
        {
            await _dbContext.AddAsync(specialization);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            await _dbContext.Specializations
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Specializations
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddMember(int id, int memberId)
        {
            var specialization = await _dbContext.Specializations.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == id);
            var member = await _dbContext.Members.FindAsync(memberId);

            if (specialization != null && member != null)
            {
                specialization.Members.Add(member);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMember(int id, int memberId)
        {
            var specialization = await _dbContext.Specializations.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == id);
            var member = await _dbContext.Members.FindAsync(memberId);

            if (specialization != null && member != null)
            {
                specialization.Members.Remove(member);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
