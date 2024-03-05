using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CityId { get; set; }
        [MaxLength(200)]
        public required string Name { get; set; }   

        public virtual City City { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
