namespace eShop.BLL.Services.src;

using Abstraction;
using Core.Entities;
using DAL.Interface;

public class CheckoutService(ICartService cartService, IUnitOfWork unitOfWork) : ICheckoutService {
    public async Task<Order?> CheckIt(string userId) {
        var items = await cartService.GetCartItemsAsync(userId);
        List<ItemInOrder> itemsInOrder = new();
        foreach (var item in items){
            itemsInOrder.Add(new()
            {
                Quantity = item.Quantity,
                Price = item.Price,
                ProductId = item.ProductId,
            });
        }
        Order order = new()
        {
            UserId = userId,
            Items = itemsInOrder
        };

        var isSuccess = await unitOfWork.Orders.AddAsync(order);
        
        if (isSuccess != null){ await cartService.ClearCartAsync(userId); }
        
        return isSuccess;
    }
}