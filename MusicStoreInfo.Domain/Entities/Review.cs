using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
