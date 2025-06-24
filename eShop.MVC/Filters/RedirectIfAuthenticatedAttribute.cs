using eShop.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.MVC.Filters
{
    public class RedirectIfAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var action = nameof(HomeController.Index);
                var controller = typeof(HomeController)
                                     .Name
                                     .Replace("Controller", "");

                context.Result = new RedirectToActionResult(action, controller, new { area = "" });
            }
            base.OnActionExecuting(context);
        }
    }
}
