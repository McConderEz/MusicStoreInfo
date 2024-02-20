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
    public class CassetteConfiguration : IEntityTypeConfiguration<Cassette>
    {
        public void Configure(EntityTypeBuilder<Cassette> builder)
        {
            builder.ToTable<Cassette>(t => t.HasCheckConstraint("Duration", "Duration > 0"));
        }
    }
}
