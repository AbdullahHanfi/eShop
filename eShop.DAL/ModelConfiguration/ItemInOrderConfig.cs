using eShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace eShop.DAL.ModelConfiguration
{
    public class ItemInOrderConfig : IEntityTypeConfiguration<ItemInOrder>
    {
        public void Configure(EntityTypeBuilder<ItemInOrder> builder)
        {
            //Adding index on both product and order
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasOne(e => e.Order)
                .WithMany(e => e.Items)
                .IsRequired(false);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Product)
                .WithMany(e => e.ItemsInOrder)
                .IsRequired(true);
            builder.HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
