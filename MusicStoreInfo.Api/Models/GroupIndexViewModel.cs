using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class GroupIndexViewModel
    {
        public List<Group> Groups { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
