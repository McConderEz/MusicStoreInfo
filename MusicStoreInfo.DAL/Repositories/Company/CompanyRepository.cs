using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public CompanyRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Company>> Get()
        {
            return await _dbContext.Companies
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.District)
                .Include(a => a.Albums)
                .Include(a => a.District)
                .ToListAsync();
        }

        public async Task<Company?> GetById(int id)
        {
            return await _dbContext.Companies
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Company company)
        {
            await _dbContext.AddAsync(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int districtId, string phoneNumber, string name)
        {
            await _dbContext.Companies
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.DistrictId, districtId)
                    .SetProperty(a => a.PhoneNumber, phoneNumber)
                    .SetProperty(a => a.Name, name));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Companies
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
