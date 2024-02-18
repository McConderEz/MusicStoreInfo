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

        public required string Name { get; set; }
        public int ListenerTypeId { get; set; }
        public int CompanyId { get; set; }  
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int SongsCount { get; set; }

        public virtual ICollection<Store> Stores { get; set; }    
        public virtual ICollection<Cassette> Cassettes { get; set;}
        public virtual ListenerType ListenerType { get; set; }
        public virtual Company Company { get; set; }

    }
}
