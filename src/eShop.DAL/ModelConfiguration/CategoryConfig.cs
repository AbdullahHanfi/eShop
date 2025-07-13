using eShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.DAL.ModelConfiguration
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.MainLayout)
                .HasDefaultValueSql("0");

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.HasMany(e => e.Products)
                .WithOne(e=>e.Category)
                .IsRequired(false);

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
