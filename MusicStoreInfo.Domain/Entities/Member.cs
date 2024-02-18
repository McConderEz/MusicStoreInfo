using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStoreInfo.Domain.Entities
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string SecondName { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set;}        
        public virtual Gender Gender { get; set; }
        public virtual ICollection<MemberGroupLink> Groups { get; set; }
        public virtual ICollection<MemberSpecializationLink> Specializations { get; set; }

    }
}