namespace eShop.BLL.ViewModels.Dashboard;

public class TopSellingProductDto {
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int TotalSold { get; set; }
    public decimal Revenue { get; set; }
    public string ImagePath { get; set; }
}