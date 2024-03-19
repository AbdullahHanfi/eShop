using eShop.DAL;
using eShop.BLL;
using eShop.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eShop.Core.Entities;
using eShop.BLL.Utilities;
using NToastNotify;
using eShop.BLL.Helper;
using eShop.DAL.Seeds;

namespace eShop.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseOnHover = true
            });
            builder.Services.AddControllersWithViews();


            //Relationanl DataBase
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<eShopDbContext>(options =>
                options.UseSqlServer(connectionString,
                Name => Name.MigrationsAssembly(typeof(eShopDbContext).Assembly.FullName)
                ));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<eShopDbContext>()
                .AddDefaultTokenProviders();

            ServiceRegisterationDAL.Add(builder.Services);
            ServiceRegisterationBLL.Add(builder.Services);
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.Configure<PhotoSettings>(builder.Configuration.GetSection("PhotoSettings"));

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //    options.LogoutPath = "/Account/Logout";
            //    options.SlidingExpiration = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //    options.Cookie = new CookieBuilder
            //    {
            //        HttpOnly = true,
            //        Name = ".lisana.Security.Cookie"
            //    };
            //});

            var app = builder.Build();

            //Seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                DefaultRoles.SeedAsync(roleManager).RunSynchronously();
                DefaultSuperAdmin.SeedAsync(userManager).RunSynchronously();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.Run();
        }
    }
}
