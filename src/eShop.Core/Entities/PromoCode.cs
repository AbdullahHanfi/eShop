namespace eShop.Core.Entities;

public class PromoCode : BaseEntity {
    public string code { get; set; }
    public bool IsUsed { get; set; }
    public decimal SalePercentage { get; set; }
}