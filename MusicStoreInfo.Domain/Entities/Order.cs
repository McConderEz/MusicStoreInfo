using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }      
        public string DeliveryPoint { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedArrivalDate { get; set; }
        public bool IsDelivered { get; set; }
        public int Quantity { get; set; }

        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AlbumId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
