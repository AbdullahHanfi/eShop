using eShop.Core.Entities;
using eShop.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace eShop.DAL.ModelConfiguration
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.imgPath)
                .IsRequired();


            builder.HasOne<Product>()
            .WithMany(e => e.Images)
            .IsRequired();

            builder.HasQueryFilter(p => !p.IsDeleted);

            //builder.HasDiscriminator<string>("ImgFor")
            //    .HasValue<Product>("Product")
            //    .HasValue<ApplicationUser>("User");

        }
    }
}
