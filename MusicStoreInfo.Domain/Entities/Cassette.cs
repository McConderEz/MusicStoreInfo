﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    //TODO: Возможное расширение - добавить Создателей(Группы)
    public class Cassette
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public required string Name { get; set; }
        public int Duration { get; set; }        

        public virtual Album Album { get; set; }
    }
}
