namespace eShop.BLL.ViewModels.Cart;

public class CartItem {
    public  Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}