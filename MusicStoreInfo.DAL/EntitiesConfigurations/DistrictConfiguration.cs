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
    internal class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.HasMany(t => t.Stores)
                .WithOne(t => t.District)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.Companies)
                .WithOne(t => t.District)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
