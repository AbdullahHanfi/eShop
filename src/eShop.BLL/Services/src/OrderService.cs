namespace eShop.BLL.Services.src;

using Abstraction;
using AutoMapper;
using Core.Entities;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ViewModels.Order;
using ViewModels.Product;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) : IOrderService {

    public async Task<IEnumerable<OrderListViewModel>> GetAllOrdersAsync() {
        var orders = await unitOfWork.Orders
            .Include(o => o.Items)
            .Include(e=>e.User)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderListViewModel
            {
                Id = o.Id,
                OrderNumber = $"ORD-{o.CreatedDate:yyyyMMdd}-{o.Id.ToString().Substring(0, 8).ToUpper()}",
                UserEmail =   o.User.Email,
                CreatedDate = o.CreatedDate,
                TotalAmount = o.Items.Sum(i => i.Price * i.Quantity),
                ItemsCount = o.Items.Count,
            })
            .ToListAsync();

        return orders;
    }

    public async Task<AdminOrderViewModel> GetOrderDetailsAsync(Guid orderId) {
        var order = await unitOfWork.Orders
            .Include(o => o.Items)
            .Where(o => o.Id == orderId)
            .FirstOrDefaultAsync();

        if (order == null)
            return null;

        var user = await userManager.FindByIdAsync(order.UserId);
        var products = new List<OrderItemViewModel>();
        foreach (var item in order.Items){
            var product = await unitOfWork.Products.FindAsync(i => i.Id == item.ProductId, i => i.Images);
            products.Add(
            new OrderItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.Images.FirstOrDefault()?.imgPath ?? "/assets/no-image.png",
                Quantity = product.Quantity,
                Price = product.CurrentPrice
            });
        }

        return new AdminOrderViewModel
        {
            Id = order.Id,
            UserId = order.UserId,
            UserEmail = user?.Email ?? "Unknown",
            UserName = user?.UserName ?? "Unknown",
            CreatedDate = order.CreatedDate,
            TotalAmount = order.Items.Sum(i => i.Price * i.Quantity),
            TotalItems = order.Items.Sum(i => i.Quantity),
            Items = products
        };
    }

    public async Task<List<OrderViewModel>> GetAllOrdersAsync(string userId) {
        var orders = await unitOfWork.Orders.FindAllAsync(e => e.UserId == userId, e => e.Items);
        List<OrderViewModel> ordersViewModel = new(orders.Count());
        foreach (var order in orders){
            ordersViewModel.Add(
            new()
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                Price = order.Items.Sum(i => i.Price * i.Quantity),
                NumberOfProducts = order.Items.Count
            }
            );
        }
        return ordersViewModel;
    }
    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(string userId, Guid orderId) {
        var order = await unitOfWork.Orders.FindAsync(e => e.UserId == userId && e.Id == orderId, e => e.Items);
        List<ProductViewModel> products = new();

        foreach (var item in order.Items){
            var product = await unitOfWork.Products.FindAsync(e => e.Id == item.ProductId, e => e.Images);
            products.Add(mapper.Map<ProductViewModel>(product));
            products.Last().Quantity = item.Quantity;
        }

        OrderDetailsViewModel orderDetailsViewModel = new()
        {
            Price = order.Items.Sum(i => i.Price * i.Quantity),
            Products = products
        };

        return orderDetailsViewModel;
    }

}