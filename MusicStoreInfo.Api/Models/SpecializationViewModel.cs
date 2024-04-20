using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Api.Models
{
    public class SpecializationViewModel
    {
        public Specialization Specialization { get; set; }
        public List<Member> Members { get; set; }
    }
}
