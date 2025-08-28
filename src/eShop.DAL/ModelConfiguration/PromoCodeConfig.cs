namespace eShop.DAL.ModelConfiguration;

using Core.Entities;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PromoCodeConfig : IEntityTypeConfiguration<PromoCode> {
    public void Configure(EntityTypeBuilder<PromoCode> builder) {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.code)
            .HasMaxLength(StringConstants.MaxSize)
            .IsRequired();

        builder.Property(e => e.IsUsed)
            .HasDefaultValueSql("0");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}