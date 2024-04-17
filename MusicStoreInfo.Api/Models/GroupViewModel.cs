using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class GroupViewModel
    {
        public Group Group { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Member> Members { get; set; }
    }
}
