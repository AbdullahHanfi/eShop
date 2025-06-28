using eShop.Core.Entities;
using eShop.Core.Utilities;
using Microsoft.AspNetCore.Identity;

namespace eShop.DAL.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            //Seed admin User
            var admintUser = new ApplicationUser
            {
                UserName = "super admin",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "user",
                Email = "user@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "user@gmail.com");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Customer);
                }
            }
            if (userManager.Users.All(u => u.Id != admintUser.Id))
            {
                var user = await userManager.FindByEmailAsync(admintUser.Email);
                if (user == null)
                {
                    var x = await userManager.CreateAsync(admintUser, "superadmin@gmail.com");
                    await userManager.AddToRoleAsync(admintUser, Roles.SuperAdmin);
                    await userManager.AddToRoleAsync(admintUser, Roles.Admin);
                    await userManager.AddToRoleAsync(admintUser, Roles.Customer);
                }
            }
        }
    }
}
