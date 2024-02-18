using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Несоответствие формату")]
        public required string PhoneNumber { get; set; }
        public required string Name { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        
    }
}
