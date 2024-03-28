using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Core.Utilities
{
    public class IdentityOperationResult<T>
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public override string ToString()
        {
            if (!Succeeded)
            {
                return $"Failed : {string.Join(" , ", Errors)}";
            }
            return "Succeeded";
        }
    }
}
