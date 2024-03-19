using eShop.BLL.Helper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.Services.src;
using eShop.Core.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.BLL
{
    public static class ServiceRegisterationBLL
    {
        public static void Add(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<PhotoSettings>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAccountServies, AccountServies>();
            services.AddScoped<IImageServices, ImageServices>();

        }
    }
}
