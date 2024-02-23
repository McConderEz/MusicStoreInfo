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
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable<Member>(t => t.HasCheckConstraint("Age", "Age > 0 AND Age < 120"));
            builder.HasIndex(t => t.Id);

            builder.HasMany(s => s.Specializations)
                .WithMany(m => m.Members)
                .UsingEntity(j => j.ToTable("MemberSpecializationLink"));

            builder.HasMany(g => g.Groups)
                .WithMany(m => m.Members)
                .UsingEntity(j => j.ToTable("MemberGroupLink"));
        }
    }
}
