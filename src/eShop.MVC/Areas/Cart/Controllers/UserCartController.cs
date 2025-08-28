using Microsoft.AspNetCore.Mvc;
namespace eShop.MVC.Areas.Cart.Controllers;

using System.Security.Claims;
using BLL.Services.Abstraction;
using BLL.ViewModels.Cart;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

[Area("Cart")]
[Route("/cart")]
[Authorize(Roles = Roles.Customer)]
public class UserCartController(ICartService cartService) : Controller {

    public async Task<IActionResult> Index() {
        var userId = GetCurrentUserId();
        var CartItems = await cartService.GetCartItemsAsync(userId);

        return View(CartItems ?? []);
    }
    
    [HttpGet("/api/cart/count")]
    public async Task<IActionResult> CartCount() {
        var userId = GetCurrentUserId();
        var cartCount = await cartService.UserCartCountAsync(userId);
        return Ok(cartCount);
    }

    [HttpPost("/api/cart/add")]
    public async Task<IActionResult> AddToCart([FromBody] CartRequest request) {
        var userId = GetCurrentUserId();
        var success = await cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);

        return success
            ? Ok(new { message = "Item added to cart" })
            : BadRequest(new { message = "Failed to add item" });
    }

    [HttpPut("/api/cart/update")]
    public async Task<IActionResult> UpdateQuantity([FromBody] CartRequest request) {
        var userId = GetCurrentUserId();
        var success = await cartService.UpdateQuantityAsync(userId, request.ProductId, request.Quantity);

        return success
            ? Ok(new { message = "Quantity updated" })
            : BadRequest(new { message = "Failed to update quantity" });
    }

    [HttpDelete("/api/cart/remove/{productId}")]
    public async Task<IActionResult> RemoveFromCart(Guid productId) {
        var userId = GetCurrentUserId();
        var success = await cartService.RemoveFromCartAsync(userId, productId);

        return success
            ? Ok(new { message = "Item removed" })
            : BadRequest(new { message = "Failed to remove item" });
    }

    [HttpDelete("/api/cart/clear")]
    public async Task<IActionResult> ClearCart() {
        var userId = GetCurrentUserId();
        await cartService.ClearCartAsync(userId);
        return Ok(new { message = "Cart cleared" });
    }
    private string GetCurrentUserId() {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Session.Anonymous;
    }

}