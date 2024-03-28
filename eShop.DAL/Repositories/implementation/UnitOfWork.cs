using eShop.Core.Entities;
using eShop.DAL.Interface;
using eShop.DAL.Data;
using Microsoft.AspNetCore.Identity;

namespace eShop.DAL.Repositories.implementation
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly eShopDbContext _context;
        public UnitOfWork(eShopDbContext context,
            IUserRepository users,
            IProductRepository product,
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

        public IProductRepository Products { get; }

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
