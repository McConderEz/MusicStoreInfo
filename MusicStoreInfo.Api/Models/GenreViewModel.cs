using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class GenreViewModel
    {
        public Genre Genre { get; set; }
        public List<Group> Groups { get; set; }
    }
}
