namespace eShop.BLL.ViewModels.Order;

using Product;

public class OrderDetailsViewModel {
    public decimal Price { get; set; }
    public List<ProductViewModel> Products { get; set; }
}