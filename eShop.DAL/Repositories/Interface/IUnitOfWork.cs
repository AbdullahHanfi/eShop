using eShop.Core.Entities;

namespace eShop.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IBaseRepository<Image> Images { get; }
        public IBaseRepository<Order> Orders { get; }
        public IBaseRepository<ItemInOrder> ItemInOrders { get; }
        int Complete();
    }
}