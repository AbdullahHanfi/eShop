using eShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.DAL.ModelConfiguration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasOne<ApplicationUser>()
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .IsRequired(false);

            builder.HasMany(e => e.Items)
                .WithOne(e => e.Order)
                .IsRequired(false);

            builder.HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
