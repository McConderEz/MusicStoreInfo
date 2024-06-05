using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class CityDistrictViewModel
    {
        public List<City> Cities { get; set; }
        public List<District> Districts { get; set; }
    }
}
