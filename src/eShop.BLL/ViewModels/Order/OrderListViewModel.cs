namespace eShop.BLL.ViewModels.Order;

public class OrderListViewModel {
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemsCount { get; set; }
}