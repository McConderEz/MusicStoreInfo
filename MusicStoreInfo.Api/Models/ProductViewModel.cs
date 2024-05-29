using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Store> Stores { get; set; }
        public List<Group> Groups { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
