using eShop.Core.Entities;
using eShop.DAL.Interface;
using eShop.DAL.Data;
using Microsoft.AspNetCore.Identity;
using eShop.DAL.Repositories.Interface;

namespace eShop.DAL.Repositories.implementation {
    public class UnitOfWork(
        eShopDbContext context,
        IUserRepository users,
        IProductRepository product,
        IBaseRepository<Image> image,
        IOrderRepository order,
        IBaseRepository<ItemInOrder> itemInOrder,
        IBaseRepository<Category> categories,
        IRoleRepository roles,
        ICartRepository carts,
        IBaseRepository<PromoCode> promoCodes)
        : IUnitOfWork {

        public IUserRepository Users { get; } = users;

        public IProductRepository Products { get; } = product;

        public IBaseRepository<Image> Images { get; } = image;

        public IOrderRepository Orders { get; } = order;

        public IBaseRepository<ItemInOrder> ItemInOrders { get; } = itemInOrder;

        public IBaseRepository<Category> Categories { get; } = categories;

        public IRoleRepository Roles { get; } = roles;

        public ICartRepository Carts { get; } = carts;
        public IBaseRepository<PromoCode> PromoCodes { get; } = promoCodes;

        public int Complete() {
            return context.SaveChanges();
        }

        public void Dispose() {
            context.Dispose();
        }
    }
}