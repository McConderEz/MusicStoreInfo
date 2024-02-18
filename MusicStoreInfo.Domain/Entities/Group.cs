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
        public required string Name { get; set; }       
        
        public virtual ICollection<GroupGenreLink> Genres { get; set; }
        public virtual ICollection<MemberGroupLink> Members { get; set; }
    }
}
