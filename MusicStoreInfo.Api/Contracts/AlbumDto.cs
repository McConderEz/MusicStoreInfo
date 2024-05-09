using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreInfo.Api.Contracts
{
    public record class AlbumDto(
        [Required][MaxLength(50)] string Name,
        [Required] int ListenerTypeId,
        [Required] int CompanyId,
        [Required] int GroupId,        
        [Required] DateTime ReleaseDate,
        IFormFile? ImagePath,
        [Required] int SongsCount = 0,
        [Required] int Duration = 0,
        int Id = 0);
}
