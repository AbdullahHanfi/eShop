using eShop.BLL.Mapping;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.Services.src;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.BLL
{
    using Core.Entities;

    public static class ServiceRegisterationBLL
    {
        public static void Add(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAccountServies, AccountServies>();
            services.AddScoped<IImageServices, ImageServices>();
            services.AddScoped<IMailTransport, MailTransport>();
            services.AddScoped<IGuidProvider, GuidProvider>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddAutoMapper(typeof(AccountProfile));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(CartProfile));
        }
    }
}
