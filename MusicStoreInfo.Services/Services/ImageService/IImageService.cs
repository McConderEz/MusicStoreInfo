using Microsoft.AspNetCore.Http;

namespace MusicStoreInfo.Services.Services.ImageService
{
    public interface IImageService
    {
        Task<string?> CreateImageAsync(IFormFile titleImage, string path);
    }
}