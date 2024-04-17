using Microsoft.AspNetCore.Http;
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
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public int ListenerTypeId { get; set; }
        public int CompanyId { get; set; }  
        public int GroupId { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int SongsCount { get; set; }
        public string? ImagePath { get; set; } = string.Empty;

        public virtual ICollection<Store>? Stores { get; set; } = [];
        public virtual ICollection<Product>? Products { get; set; } = [];
        public virtual ICollection<Song>? Songs { get; set; } = [];
        public virtual Group? Group { get; set; }
        public virtual ListenerType? ListenerType { get; set; }
        public virtual Company? Company { get; set; }

    }
}
