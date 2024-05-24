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
    public class ShoppingCartProductLinkConfiguration : IEntityTypeConfiguration<ShoppingCartProductLink>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartProductLink> builder)
        {
            builder.HasKey(scp => new {scp.ShoppingCartId, scp.ProductId});

            builder.HasOne(scp => scp.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartProducts)
                .HasForeignKey(scp => scp.ShoppingCartId);

            builder.HasOne(scp => scp.Product)
                .WithMany(sc => sc.ShoppingCartProducts);
        }
    }
}
