using Hangfire.Dashboard;
using eShop.Core.Utilities;
namespace eShop.MVC.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole(Roles.SuperAdmin);
        }
    }
}
