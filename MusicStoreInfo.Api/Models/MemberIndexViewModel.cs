using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class MemberIndexViewModel
    {
        public List<Member> Members { get; set; }
        public List<Group> Groups { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Specialization> Specializations { get; set; }
    }
}
