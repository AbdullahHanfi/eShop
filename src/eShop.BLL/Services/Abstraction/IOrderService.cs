namespace eShop.BLL.Services.Abstraction;

using ViewModels.Order;

public interface IOrderService {
    Task<IEnumerable<OrderListViewModel>> GetAllOrdersAsync();
    Task<AdminOrderViewModel> GetOrderDetailsAsync(Guid orderId);
    Task<List<OrderViewModel>> GetAllOrdersAsync(string userId);
    Task<OrderDetailsViewModel> GetOrderDetailsAsync(string userId, Guid orderId);
}