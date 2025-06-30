using eShop.BLL.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.src
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
