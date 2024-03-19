using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Core.Utilities
{
    public static class Roles
    {
        public static string SuperAdmin { get; } = "SuperAdmin";
        public static string Admin { get; } = "Admin";
        public static string Customer { get; } = "Customer";
    }
}
