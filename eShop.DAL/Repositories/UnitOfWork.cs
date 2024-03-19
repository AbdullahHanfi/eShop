using eShop.Core.Entities;
using eShop.Core.Interface;
using eShop.DAL.Data;
using Microsoft.AspNetCore.Identity;

namespace eShop.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly eShopDbContext _context;
        public UnitOfWork(eShopDbContext context,
            IUserRepository users,
            IBaseRepository<Product> product,
            IBaseRepository<Image> Image,
            IBaseRepository<Order> Order,
            IBaseRepository<ItemInOrder> ItemInOrder)
        {
            _context = context;
            Users = users;
            Products = product;
            Images = Image;
            Orders = Order;
            ItemInOrders = ItemInOrder;
        }
        public IUserRepository Users { get; }

        public IBaseRepository<Product> Products { get; }

        public IBaseRepository<Image> Images { get; }

        public IBaseRepository<Order> Orders { get; }

        public IBaseRepository<ItemInOrder> ItemInOrders { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
