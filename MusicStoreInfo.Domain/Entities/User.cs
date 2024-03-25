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
        [MaxLength(25), MinLength(3)]
        public string Name { get; set; }
        [MaxLength(25), MinLength(6)]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
