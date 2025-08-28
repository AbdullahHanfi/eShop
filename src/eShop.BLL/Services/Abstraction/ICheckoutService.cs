namespace eShop.BLL.Services.Abstraction;

using Core.Entities;

public interface ICheckoutService {
    Task<Order?> CheckIt(string userId);
}