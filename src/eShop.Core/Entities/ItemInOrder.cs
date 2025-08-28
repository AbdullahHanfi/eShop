namespace eShop.Core.Entities {
    public class ItemInOrder : BaseEntity {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }  
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}