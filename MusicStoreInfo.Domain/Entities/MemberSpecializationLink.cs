using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class MemberSpecializationLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberSpecializationLinkId { get; set; }
        public int MemberId { get; set;}
        public int SpecializationId { get; set; }
        public virtual Member Member { get; set; }
        public virtual Specialization Specialization { get; set; }  
    }
}
