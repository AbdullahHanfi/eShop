namespace eShop.DAL.ModelConfiguration;

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VisitorConfig : IEntityTypeConfiguration<Visitor> {
    public void Configure(EntityTypeBuilder<Visitor> builder) {
        builder.HasKey(x => x.IpAddress);

        builder.Property(e => e.IpAddress)
            .HasDefaultValueSql("NEWID()");

        builder.OwnsMany(v => v.Histories, Builder =>
        {
            Builder.WithOwner().HasForeignKey("VisitorIpAddress"); // shadow FK
            Builder.HasKey(e => e.Id);

            Builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            Builder.Property(e => e.UserAgent).HasMaxLength(500);
            Builder.Property(e => e.Path).HasMaxLength(2000);
            Builder.Property(e => e.Referrer).HasMaxLength(2000);
        });
    }
}