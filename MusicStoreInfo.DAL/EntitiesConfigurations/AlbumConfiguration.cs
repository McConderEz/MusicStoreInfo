﻿using Microsoft.EntityFrameworkCore;
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
            builder.ToTable<Album>(t => t.HasCheckConstraint("SongsCountConstraint", "SongsCount >= 0"))
                   .ToTable<Album>(t => t.HasCheckConstraint("DurationAlbum", "Duration >= 0"))
                   .ToTable<Album>(t => t.HasCheckConstraint("ReleaseDate", $"YEAR(ReleaseDate) <= YEAR(GETDATE())"));

            builder.HasIndex(a => a.GroupId);

            builder.HasOne(a => a.Group)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.GroupId);

            builder.HasOne(a => a.Company)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.CompanyId);

            builder.HasOne(a => a.ListenerType)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.ListenerTypeId);

            builder.HasMany(a => a.Songs)
                .WithOne()
                .HasForeignKey(s => s.AlbumId);           

            builder.HasMany(a => a.Stores)
                .WithMany(s => s.Albums)
                .UsingEntity<Product>(j => j.HasOne(t => t.Store)
                                            .WithMany(t => t.Products)
                                            .HasForeignKey(k => k.StoreId),
                                      j => j.HasOne(t => t.Album)
                                            .WithMany(t => t.Products)
                                            .HasForeignKey(k => k.AlbumId)
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
