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
            builder.HasOne(t => t.City)
                .WithMany(t => t.Districts)
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Stores)
                .WithOne(t => t.District)
                .HasForeignKey(t => t.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Companies)
                .WithOne(t => t.District)
                .HasForeignKey(t => t.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
