using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Core.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public DateTime? CreatedDate { get; set; }
    }
}
