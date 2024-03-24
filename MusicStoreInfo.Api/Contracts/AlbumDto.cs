using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreInfo.Api.Contracts
{
    public record class AlbumDto(
        [Required][MaxLength(50)] string Name,
        [Required] int ListenerTypeId,
        [Required] int CompanyId,
        [Required] int GroupId,
        [Required] int Duration,
        [Required] DateTime ReleaseDate,
        [Required] int SongsCount,
        IFormFile ImagePath);
}
