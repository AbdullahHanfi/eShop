using eShop.Core.Entities;
using eShop.Core.Utilities;
using Microsoft.AspNetCore.Identity;

namespace eShop.DAL.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.SuperAdmin,
                NormalizedName = Roles.SuperAdmin.ToUpper(),
                CreatedDate = DateTime.Now
            });
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
                CreatedDate = DateTime.Now
            });
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Customer,
                NormalizedName = Roles.Customer.ToUpper(),
                CreatedDate = DateTime.Now
            });
        }
    }
}
