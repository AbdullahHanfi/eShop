namespace eShop.BLL.ViewModels.Order;

public class AdminOrderViewModel {
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public List<OrderItemViewModel> Items { get; set; }
}