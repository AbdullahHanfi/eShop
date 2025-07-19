using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.Abstraction
{
    public interface IRoleService
    {
        public Task GrantRoleTemporarilyAsync(Guid userId,string role,TimeSpan duration);
        public Task RevokeRoleAsync(Guid userId, string role);
    }
}
