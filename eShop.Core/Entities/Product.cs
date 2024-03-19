﻿
namespace eShop.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Rate { get; set; }
        public decimal CurentPrice { get; set; }
        public decimal? PrevPrice { get; set; }
        public virtual ICollection<ItemInOrder> ItemsInOrder { get; set; } = new HashSet<ItemInOrder>();
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
