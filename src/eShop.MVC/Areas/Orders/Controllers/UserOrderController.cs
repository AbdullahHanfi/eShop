using Microsoft.AspNetCore.Mvc;
namespace eShop.MVC.Areas.Orders.Controllers;

using System.Security.Claims;
using BLL.Services.Abstraction;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;

[Area("Orders")]
[Route("/orders")]
[Authorize(Roles = Roles.Customer)]
public class UserOrderController(IOrderService orderService) : Controller {

    public async Task<IActionResult> Index()
        => View(await orderService.GetAllOrdersAsync(GetCurrentUserId()));

    [HttpGet("{orderId:Guid}")]
    public async Task<IActionResult> Details(Guid orderId)
        => View(await orderService.GetOrderDetailsAsync(GetCurrentUserId(), orderId));


    private string GetCurrentUserId() {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Session.Anonymous;
    }
}