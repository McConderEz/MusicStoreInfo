using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.EntitiesConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint("Price", "Price > 0 AND Price <= 1000000"))
                .ToTable(t => t.HasCheckConstraint("Quantity", "Quantity >= 0"))
                .ToTable(t => t.HasCheckConstraint("DateReceived", "DateReceived <= GETDATE()"));
            builder.HasIndex(t => t.Id);
        }
    }
}
