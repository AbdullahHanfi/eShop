namespace eShop.BLL.ViewModels.Dashboard;

public class RecentOrderDto {
    public Guid OrderId { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
}