namespace eShop.BLL.ViewModels.Dashboard;

public class DashboardViewModel {
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalCategories { get; set; }
    public int TotalUsers { get; set; }
    
    public decimal TodayRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
    
    public List<RecentOrderDto> RecentOrders { get; set; }
    public List<LowStockProductDto> LowStockProducts { get; set; }
    public List<TopSellingProductDto> TopSellingProducts { get; set; }
    
    public RevenueChartDataDto RevenueChartData { get; set; }
}