using eShop.Core.Entities;

namespace eShop.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public IBaseRepository<Product> Products { get; }
        public IBaseRepository<Image> Images { get; }
        public IBaseRepository<Order> Orders { get; }
        public IBaseRepository<ItemInOrder> ItemInOrders { get; }
        int Complete();
    }
}