
namespace eShop.Core.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }           
        public bool MainLayout { get; set; }           
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
