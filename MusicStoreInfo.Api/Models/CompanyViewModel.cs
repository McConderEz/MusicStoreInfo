using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class CompanyViewModel
    {
        public List<Company> Companies { get; set; }
        public List<City> Cities { get; set; }
    }
}
