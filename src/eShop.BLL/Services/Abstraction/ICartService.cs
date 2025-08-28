namespace eShop.BLL.Services.Abstraction;

using ViewModels.Cart;

public interface ICartService {
    Task<List<CartItem>> GetCartItemsAsync(string userId);
    Task<bool> AddToCartAsync(string userId, Guid productId, int quantity);
    Task<bool> UpdateQuantityAsync(string userId, Guid productId, int quantity);
    Task<bool> RemoveFromCartAsync(string userId, Guid productId);
    Task<CartSummary> GetCartSummaryAsync(string userId);
    Task<int> UserCartCountAsync(string userId);
    Task ClearCartAsync(string userId);
}