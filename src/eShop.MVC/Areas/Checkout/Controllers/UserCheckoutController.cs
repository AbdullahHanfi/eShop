using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Areas.CheckOut.Controllers;

using System.Security.Claims;
using BLL.Services.Abstraction;
using Checkout.Model;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;

[Area("Checkout")]
[Route("/Checkout")]
[Authorize(Roles = Roles.Customer)]
public class UserCheckoutController(ICheckoutService checkoutService) : Controller {
    [HttpGet]
    public async Task<ActionResult> Index() {
        var userId = GetCurrentUserId();
        var isSuccess = await checkoutService.CheckIt(userId);
        if (isSuccess is not null){
            CheckoutSuccessModel model = new()
            {
                OrderNumber = isSuccess.Id.ToString(),
                OrderDate = isSuccess.CreatedDate,
                ItemCount = isSuccess.Items.Count,
                TotalAmount = isSuccess.Items.Sum(i => i.Price),
            };
            return View("OrderSuccessful", model);
        }
        return View("Error");
    }
    private string GetCurrentUserId() {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Session.Anonymous;
    }
}