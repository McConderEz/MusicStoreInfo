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
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string SecondName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int GenderId { get; set;}        
        public virtual Gender? Gender { get; set; }
        public virtual ICollection<Group>? Groups { get; set; } = [];
        public virtual ICollection<Specialization>? Specializations { get; set; } = [];

    }
}