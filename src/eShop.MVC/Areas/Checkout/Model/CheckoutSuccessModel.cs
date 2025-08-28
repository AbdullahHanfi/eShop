namespace eShop.MVC.Areas.Checkout.Model;

public class CheckoutSuccessModel {
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime EstimatedDelivery { get; set; }
}