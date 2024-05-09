using System.ComponentModel.DataAnnotations;

namespace MusicStoreInfo.Api.Contracts
{
    public record class GroupDto([Required][MaxLength(50)] string Name,
                                IFormFile? ImagePath,
                                int Id = 0);

}
