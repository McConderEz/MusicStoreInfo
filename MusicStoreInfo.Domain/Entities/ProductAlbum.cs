﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class ProductAlbum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
        public DateTime DateReceived { get; set; }  
        public int Quantity { get; set; }   
        public decimal Price { get;set; }
    }
}
