using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.EntitiesConfigurations
{
    public class ListenerTypeConfiguration : IEntityTypeConfiguration<OwnershipType>
    {
        public void Configure(EntityTypeBuilder<OwnershipType> builder)
        {
            builder.HasMany(t => t.Stores)
                .WithOne(t => t.OwnershipType)
                .HasForeignKey(t => t.OwnershipTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
