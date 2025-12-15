using eShop.BLL;
using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL;
using eShop.DAL.Data;
using eShop.DAL.Seeds;
using eShop.DAL.Utilities;
using eShop.MVC.Filters;
using eShop.MVC.Infrastructure;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace eShop.MVC {
    using System.Text.Json;
    using Middlewares;
    using Serilog;

    public class Program {
        public static async Task Main(string[] args) {
            Console.WriteLine($"Tunnel URL: " +
                              $"{Environment.GetEnvironmentVariable("VS_TUNNEL_URL")}");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseOnHover = true
            });
            builder.Services.AddControllersWithViews().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });
            builder.Services.AddMemoryCache();
            builder.Services.AddHangfire(configuration => { configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")); });
            builder.Services.AddHangfireServer();
            //builder.Services.AddSingleton<RequestResponseLoggingMiddleware>();
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

            builder.Services.AddAuthorization(options => {
                options.AddPolicy("GuestOrCustomer",
                policy =>
                    policy.RequireAssertion(context =>
                        !context.User.Identity.IsAuthenticated ||
                        context.User.IsInRole(Roles.Customer)
                    ));
            });

            ServiceRegisterationDAL.Add(builder.Services);
            builder.Services.Configure<PhotoSettings>(builder.Configuration.GetSection("PhotoSettings"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            ServiceRegisterationBLL.Add(builder.Services);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IBackgroundJobServices, HangfireServices>();
            builder.Services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                //options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".lisana.Security.Cookie"
                };
            });
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope()){
                var db = scope.ServiceProvider.GetRequiredService<eShopDbContext>();
                db.Database.Migrate();
            }

            //Seed
            await Task.Run(async () => {
                using var scope = app.Services.CreateScope();

                var services = scope.ServiceProvider;
                var Db = scope.ServiceProvider.GetRequiredService<eShopDbContext>();

                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await DefaultRoles.SeedAsync(roleManager);
                await DefaultUsers.SeedAsync(userManager);
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()){
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            //app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            //               ForwardedHeaders.XForwardedProto
            //});
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<VisitorLoggingMiddleware>();
            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseHangfireDashboard("/dashboard",
            new DashboardOptions
            {
                Authorization = [new HangfireAuthorizationFilter()],
            });

            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}