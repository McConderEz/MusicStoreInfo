using Microsoft.AspNetCore.Http;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ImageService
{
    public class ImageService : IImageService
    {
        public async Task<string?> CreateImageAsync(IFormFile? titleImage, string path)
        {
            try
            {
                if(titleImage == null)
                {
                    return null;
                }

                var fileName = Path.GetFileName(titleImage.FileName);
                var filePath = Path.Combine(path, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await titleImage.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка обработки файла!");
            }
        }

    }
}
