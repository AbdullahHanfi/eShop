using Microsoft.AspNetCore.Identity;

namespace eShop.Core.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string imgPath { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
