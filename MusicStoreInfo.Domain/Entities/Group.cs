using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public byte[]? Image { get; set; } = [];

        public virtual ICollection<Genre>? Genres { get; set; } = [];
        public virtual ICollection<Member>? Members { get; set; } = [];
        public virtual ICollection<Album>? Albums { get; set; } = [];
    }
}
