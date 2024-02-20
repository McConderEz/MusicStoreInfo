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
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable<Album>(t => t.HasCheckConstraint("Duration", "Duration > 0"));
            builder.ToTable<Album>(t => t.HasCheckConstraint("SongsCount", "SongsCount > 0"));
            builder.ToTable<Album>(t => t.HasCheckConstraint("DateTime", $"DateTime <= {DateTime.Now.Year}"));
        }
    }
}
