namespace eShop.BLL.Services.Abstraction;

using ViewModels.Dashboard;

public interface IDashboardServices {
    Task<DashboardViewModel> RetrieveDashboardDataAsync();
    Task<decimal> GetTodayRevenueAsync();
    Task<decimal> GetMonthlyRevenueAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<List<RecentOrderDto>> GetRecentOrdersAsync();
    Task<List<LowStockProductDto>> GetLowStockProductsAsync();
    Task<List<TopSellingProductDto>> GetTopSellingProductsAsync();
    Task<RevenueChartDataDto> GetRevenueChartDataAsync();
}