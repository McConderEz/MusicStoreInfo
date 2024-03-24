using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class AlbumViewModel
    {
        public Album Album { get; set; }
        public List<Song>? Songs { get; set; }
        public List<Product>? Products { get; set; }
        public List<Store>? Stores { get; set; }
    }
}
