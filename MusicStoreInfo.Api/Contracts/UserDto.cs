using System.ComponentModel.DataAnnotations;

namespace MusicStoreInfo.Api.Contracts
{
    public record class UserDto([Required] string UserName,
                                [Required] string PasswordHash,
                                string? Email,
                                string? PhoneNumber,
                                IFormFile? ImagePath,
                                int Id = 0,
                                int RoleId = 0);

}
