using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class StoreViewModel
    {
        public List<Store> Stores { get; set; }
        public List<OwnershipType> OwnershipTypes { get; set; }
        public List<City> Cities { get; set; }
    }
}
