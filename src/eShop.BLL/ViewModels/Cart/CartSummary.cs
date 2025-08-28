namespace eShop.BLL.ViewModels.Cart;

public class CartSummary {
    public List<CartItem> Items { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public int TotalItems { get; set; }
}