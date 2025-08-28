namespace eShop.BLL.ViewModels.Dashboard;

public class LowStockProductDto {
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int CurrentStock { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
}