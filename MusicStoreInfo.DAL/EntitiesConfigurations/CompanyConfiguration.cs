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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasMany(t => t.Albums)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId);

            builder.HasOne(t => t.District)
                .WithMany(t => t.Companies)
                .HasForeignKey(t => t.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
