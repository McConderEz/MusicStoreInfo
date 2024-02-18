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

        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Несоответствие формату")]
        public required string PhoneNumber { get; set; }
        public required string Name { get; set; }
        public DateTime YearOpened { get; set; }
        public virtual ICollection<ProductAlbum> ProductAlbums { get; set; }
        public virtual OwnershipType OwnershipType { get; set; }

    }
}
