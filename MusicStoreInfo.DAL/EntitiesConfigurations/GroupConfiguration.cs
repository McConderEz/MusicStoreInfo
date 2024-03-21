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
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasIndex(t => t.Id);

            builder.HasMany(m => m.Members)
                .WithMany(m => m.Groups)
                .UsingEntity(j => j.ToTable("MemberGroupLink"));

            builder.HasMany(g => g.Genres)
                .WithMany(g => g.Groups)
                .UsingEntity(j => j.ToTable("GroupGenreLink"));

            builder.HasMany(g => g.Albums)
                .WithOne(a => a.Group)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
