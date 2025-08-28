using eShop.Core.Entities;

namespace eShop.DAL.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<IList<ApplicationRole>> GetAllAsync();
    }
}
