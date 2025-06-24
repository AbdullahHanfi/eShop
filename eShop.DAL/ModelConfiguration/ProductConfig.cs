using eShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eShop.Core.Utilities;
using System.Reflection.Emit;

namespace eShop.DAL.ModelConfiguration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.Name)
                .HasMaxLength(StringConstants.MaxSize)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(StringConstants.MaxSize)
                .IsRequired();

            builder.Property(p => p.CurrentPrice)
            .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PrevPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(e => e.Images)
                .WithOne()
                .HasForeignKey(e => e.TargetId)
                .IsRequired();

            builder.HasMany(e => e.ItemsInOrder)
                .WithOne(e => e.Product)
                .IsRequired(false);

            builder.HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
