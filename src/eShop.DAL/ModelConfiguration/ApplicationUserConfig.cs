using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eShop.Core.Entities;
using eShop.Core.Utilities;

namespace eShop.DAL.ModelConfiguration
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.HasOne(e => e.Image)
            //    .WithOne()
            //    .HasForeignKey<Image>(id => id.TargetId)
            //    .IsRequired(false);
            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasMany(e => e.Orders)
                .WithOne()
                .IsRequired(false);

            builder.Property(e => e.imgPath)
               .HasMaxLength(StringConstants.MaxSize)
               .IsRequired(false);
            //builder.HasData(new ApplicationUser()
            //{
            //    EmailConfirmed = true,
            //    Email = "superadmin@admin.com",
            //    NormalizedEmail = "SUPERADMIN@ADMIN.COM",
            //    UserName = "admin",
            //    NormalizedUserName = "ADMIN",
            //    PhoneNumberConfirmed = true,

            //});
        }
    }
}
