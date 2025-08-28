using eShop.Core.Entities;
using eShop.DAL.Data;
using eShop.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace eShop.DAL.Repositories.implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly eShopDbContext _context;

        public RoleRepository(eShopDbContext context)
        {
            _context = context;
        }
        public async Task<IList<ApplicationRole>> GetAllAsync()
            => await _context.Roles.ToListAsync();
    }
}
