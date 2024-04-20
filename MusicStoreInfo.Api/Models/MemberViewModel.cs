using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class MemberViewModel
    {
        public Member Member { get; set; }
        public List<Group> Groups { get; set; }
        public List<Specialization> Specializations { get; set; }   
    }
}
