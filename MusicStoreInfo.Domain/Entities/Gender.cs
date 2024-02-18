using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Domain.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
