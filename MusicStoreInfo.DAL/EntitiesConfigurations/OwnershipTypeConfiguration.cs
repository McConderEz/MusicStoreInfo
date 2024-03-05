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
    public class OwnershipTypeConfiguration : IEntityTypeConfiguration<OwnershipType>
    {
        public void Configure(EntityTypeBuilder<OwnershipType> builder)
        {
            builder.HasMany(t => t.Stores)
                .WithOne(t => t.OwnershipType)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
