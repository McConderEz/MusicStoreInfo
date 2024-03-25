using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MusicStoreDbContext _dbContext;

        public MemberRepository(MusicStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Member>> Get()
        {
            return await _dbContext.Members
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Gender)
                .ToListAsync();
        }

        public async Task<Member?> GetById(int id)
        {
            return await _dbContext.Members
                .Include(a => a.Groups)
                .Include(a => a.Specializations)
                .Include(a => a.Gender)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Member member)
        {
            await _dbContext.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string name, string secondName, int age, int genderId)
        {
            await _dbContext.Members
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(a => a.Name, name)
                    .SetProperty(a => a.SecondName, secondName)
                    .SetProperty(a => a.Age, age)
                    .SetProperty(a => a.GenderId, genderId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Members
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}
