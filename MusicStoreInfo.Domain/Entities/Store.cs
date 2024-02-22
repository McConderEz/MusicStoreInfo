using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OwnershipTypeId { get; set; }
        public int DistrictId { get; set; }
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Несоответствие формату")]
        public required string PhoneNumber { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        public DateTime YearOpened { get; set; }


        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual OwnershipType OwnershipType { get; set; }
        public virtual District District { get; set; }

    }
}
