
namespace eShop.Core.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public virtual ICollection<ItemInOrder> Items { get; set; } = new HashSet<ItemInOrder>();
    }
}
