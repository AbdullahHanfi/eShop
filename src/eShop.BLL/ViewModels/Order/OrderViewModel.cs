namespace eShop.BLL.ViewModels.Order;

public class OrderViewModel {
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal Price  { get; set; }
    public int NumberOfProducts  { get; set; }
}
