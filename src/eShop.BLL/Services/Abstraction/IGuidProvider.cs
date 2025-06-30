using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.Abstraction
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}
