namespace eShop.Core.Entities;

public class Visitor {
    public string IpAddress { get; set; }
    public virtual ICollection<VisitorHistory> Histories { get; set; } = new HashSet<VisitorHistory>();
}