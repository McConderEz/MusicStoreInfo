using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class AlbumIndexViewModel
    {
        public List<Album> Albums { get; set; }
        public List<Song>? Songs { get; set; }
        public List<Group> Groups { get; set; }
        public List<Genre> Genres { get; set; }
        public List<ListenerType> ListenerTypes { get; set; }
    }
}
