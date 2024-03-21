using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IMemberRepository
    {
        Task Add(Member member);
        Task Delete(int id);
        Task<List<Member>> Get();
        Task<Member?> GetById(int id);
        Task Update(int id, string name, string secondName, int age, int genderId);
    }
}