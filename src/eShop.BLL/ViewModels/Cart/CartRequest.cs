namespace eShop.BLL.ViewModels.Cart;


public class CartRequest {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}