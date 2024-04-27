
namespace MusicStoreInfo.Services.Services
{
    public interface IAccountService
    {
        Task<string> Login(string userName, string password);
        Task Register(string useName, string password);
    }
}