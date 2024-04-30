using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50), MinLength(3)]
        public string UserName { get; set; }
        [MinLength(6)]
        public string PasswordHash { get; set; }
        public string? ImagePath { get; set; } = null;
        public string? Email { get; set; } = null;
        [RegularExpression(@"(^\+\d{1,3}\d{10}$|^$)", ErrorMessage = "Несоответствие формату")]
        public string? PhoneNumber { get; set; } = null;

        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;

        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
