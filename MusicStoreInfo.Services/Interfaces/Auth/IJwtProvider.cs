using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}