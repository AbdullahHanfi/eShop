
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Core.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<ItemInOrder> Items { get; set; } = new HashSet<ItemInOrder>();
    }
}
