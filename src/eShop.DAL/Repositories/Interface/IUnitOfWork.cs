using eShop.Core.Entities;
using eShop.DAL.Repositories.Interface;

namespace eShop.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IRoleRepository Roles{ get; }
        public IBaseRepository<Image> Images { get; }
        public IBaseRepository<Order> Orders { get; }
        public IBaseRepository<ItemInOrder> ItemInOrders { get; }
        public IBaseRepository<Category> Categories { get; }
        int Complete();
    }
}