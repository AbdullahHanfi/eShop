namespace eShop.Core.Entities;

public class Cart : BaseEntity {
    public int Quantity { get; set; }
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}