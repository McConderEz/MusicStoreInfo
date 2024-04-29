
using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services
{
    public interface IAccountService
    {
        Task<(string, User)> Login(string userName, string password);
        Task Register(string useName, string password);
        Task<User> GetUserByNameAsync(string userName);
        Task<User> GetUserByIdAsync(int id);
        Task EditAsync(int id, User model);
        Task<List<User>> GetAsync();
    }
}