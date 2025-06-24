
using Microsoft.AspNetCore.Http;

namespace eShop.BLL.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Rate { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? PrevPrice { get; set; }
        public List<byte[]> Images { get; set; } = new();
    }
}
