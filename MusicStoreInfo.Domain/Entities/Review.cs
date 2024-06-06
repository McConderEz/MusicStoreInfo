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
        public DateTime TimeCreated { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int AlbumId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
