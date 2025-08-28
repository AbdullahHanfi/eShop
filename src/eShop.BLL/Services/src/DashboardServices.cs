namespace eShop.BLL.Services.src;

using Abstraction;
using DAL.Interface;
using ViewModels.Dashboard;

public class DashboardServices(IUnitOfWork unitOfWork) : IDashboardServices {

    public async Task<DashboardViewModel> RetrieveDashboardDataAsync() {
        var viewModel = new DashboardViewModel
        {
            // Overview Statistics
            TotalProducts = await unitOfWork.Products.CountAsync(),
            TotalOrders = await unitOfWork.Orders.CountAsync(),
            TotalCategories = await unitOfWork.Categories.CountAsync(),
            TotalUsers = await unitOfWork.Users.CountAsync(),

            // Revenue Statistics
            TodayRevenue = await GetTodayRevenueAsync(),
            MonthlyRevenue = await GetMonthlyRevenueAsync(),
            TotalRevenue = await GetTotalRevenueAsync(),

            // Recent Orders
            RecentOrders = await GetRecentOrdersAsync(),

            // Low Stock Products
            LowStockProducts = await GetLowStockProductsAsync(),

            // Top Selling Products
            TopSellingProducts = await GetTopSellingProductsAsync(),

            // Revenue Chart Data
            RevenueChartData = await GetRevenueChartDataAsync()
        };
        return viewModel;
    }
    public async Task<decimal> GetTodayRevenueAsync() {
        var today = DateTime.Today;
        return (await unitOfWork.ItemInOrders
                .FindAllAsync(i => i.Order.CreatedDate.Date == today)
            ).Sum(i => i.Price * i.Quantity);
    }


    public async Task<decimal> GetMonthlyRevenueAsync() {
        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        return (await unitOfWork.ItemInOrders
                .FindAllAsync(i => i.Order.CreatedDate >= firstDayOfMonth)
            ).Sum(i => i.Price * i.Quantity);
    }

    public async Task<decimal> GetTotalRevenueAsync() {
        return (await unitOfWork.ItemInOrders
                .GetAllAsync()
            ).Sum(i => i.Price * i.Quantity);
    }

    public async Task<List<RecentOrderDto>> GetRecentOrdersAsync() {
        return unitOfWork.Orders
            .Include(i => i.Items)
            .OrderByDescending(o => o.CreatedDate)
            .Take(5)
            .Select(o => new RecentOrderDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                UserEmail = unitOfWork.Users.FindByIdAsync(o.UserId).Result.Email,
                OrderDate = o.CreatedDate,
                TotalAmount = o.Items.Sum(i => i.Price * i.Quantity),
                ItemCount = o.Items.Count
            })
            .ToList();
    }

    public async Task<List<LowStockProductDto>> GetLowStockProductsAsync() {
        return (await unitOfWork.Products
                .FindAllAsync(p => p.Active && p.Quantity <= 10, c => c.Category))
            .OrderBy(p => p.Quantity)
            .Take(5)
            .Select(p => new LowStockProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                CurrentStock = p.Quantity,
                Price = p.CurrentPrice,
                CategoryName = p.Category.Name
            })
            .ToList();
    }

    public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync() {
        return unitOfWork.Products
            .Include(i => i.ItemsInOrder, i => i.Images)
            .Select(p => new TopSellingProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                TotalSold = p.ItemsInOrder.Sum(i => i.Quantity),
                Revenue = p.ItemsInOrder.Sum(i => i.Price * i.Quantity),
                ImagePath = p.Images.FirstOrDefault().imgPath
            })
            .OrderByDescending(p => p.TotalSold)
            .Take(5)
            .ToList();
    }

    public async Task<RevenueChartDataDto> GetRevenueChartDataAsync() {
        var last30Days = DateTime.Today.AddDays(-29);
        var dailyRevenue = (await unitOfWork.ItemInOrders
                .FindAllAsync(i => i.Order.CreatedDate >= last30Days, i => i.Order)
            ).GroupBy(i => i.Order.CreatedDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                Revenue = g.Sum(i => i.Price * i.Quantity)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return new RevenueChartDataDto
        {
            Labels = dailyRevenue.Select(d => d.Date.ToString("MMM dd")).ToList(),
            Data = dailyRevenue.Select(d => d.Revenue).ToList()
        };
    }
}