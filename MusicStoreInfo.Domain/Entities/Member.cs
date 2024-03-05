using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStoreInfo.Domain.Entities
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public required string SecondName { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set;}        
        public virtual Gender Gender { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }

    }
}