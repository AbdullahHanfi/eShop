using eShop.MVC.Areas.Auth.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.MVC.Filters
{
    public class RedirectIfNotAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult(nameof(AuthController.Login), null, null);
            }

            base.OnActionExecuting(context);
        }
    }
}
