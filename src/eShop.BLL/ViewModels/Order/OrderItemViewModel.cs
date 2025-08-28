namespace eShop.BLL.ViewModels.Order;

public class OrderItemViewModel {
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price;
}