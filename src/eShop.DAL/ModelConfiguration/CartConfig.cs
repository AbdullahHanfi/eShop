namespace eShop.DAL.ModelConfiguration;

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.HasOne(e => e.User);

        builder.HasOne(e => e.Product);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

