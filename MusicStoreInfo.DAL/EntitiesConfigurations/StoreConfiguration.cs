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
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable<Store>(t => t.HasCheckConstraint("YearOpened", $"YEAR(YearOpened) <= YEAR(GETDATE())"));
            builder.HasIndex(t => t.Id);
            
            builder.HasOne(s => s.District)
                .WithMany(d => d.Stores)
                .HasForeignKey(s => s.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.OwnershipType)
                .WithMany(o => o.Stores)
                .HasForeignKey(s => s.OwnershipTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Albums)
                .WithMany(s => s.Stores)
                .UsingEntity<Product>(j => j.HasOne(t => t.Album)
                                            .WithMany(t => t.Products)
                                            .HasForeignKey(k => k.AlbumId),
                                      j => j.HasOne(t => t.Store)
                                            .WithMany(t => t.Products)
                                            .HasForeignKey(k => k.StoreId)
                                            .OnDelete(DeleteBehavior.Restrict),
                                      j =>
                                      {
                                          j.Property(pt => pt.Price);
                                          j.HasKey(t => new { t.StoreId, t.AlbumId });
                                          j.ToTable("Product");

                                          j.Property(pt => pt.Quantity);
                                          j.HasKey(t => new { t.StoreId, t.AlbumId });
                                          j.ToTable("Product");

                                          j.Property(pt => pt.DateReceived);
                                          j.HasKey(t => new { t.StoreId, t.AlbumId });
                                          j.ToTable("Product");

                                      });

        }
    }
}
