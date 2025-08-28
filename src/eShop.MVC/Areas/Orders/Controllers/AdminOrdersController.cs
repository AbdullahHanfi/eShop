namespace eShop.MVC.Areas.Orders.Controllers;

using BLL.Services.Abstraction;
using BLL.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
[Route("admin/orders")]
[Area("orders")]
public class AdminOrdersController(IOrderService orderService, ILogger<AdminOrdersController> logger) : Controller {

    public async Task<IActionResult> Index() {
        try{
            var orders = await orderService.GetAllOrdersAsync();
            return View(orders);
        }
        catch (Exception ex){
            logger.LogError(ex, "Error retrieving orders");
            TempData["Error"] = "An error occurred while loading orders.";
            return View(new List<OrderListViewModel>());
        }
    }
    [HttpGet("details/{id:Guid}")]
    public async Task<IActionResult> Details(Guid id) {

        try{
            var order = await orderService.GetOrderDetailsAsync(id);
            if (order == null){
                TempData["Error"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        catch (Exception ex){
            logger.LogError(ex, "Error retrieving order details for {OrderId}", id);
            TempData["Error"] = "An error occurred while loading order details.";
            return RedirectToAction(nameof(Index));
        }
    }
}