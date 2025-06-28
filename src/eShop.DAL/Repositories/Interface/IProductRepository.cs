
using eShop.Core.Entities;
using eShop.DAL.Repositories.Interface;

namespace eShop.DAL.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Image GetProductImage(Guid productId);
        public Task<Image> GetProductImageAsync(Guid productId);
        public IEnumerable<Image> GetProductImages(Guid productId);
        public Task<IEnumerable<Image>> GetProductImagesAsync(Guid productId);

        public IEnumerable<Order> GetProductOrders(Guid productId);
        public Task<IEnumerable<Order>> GetProductOrdersAsync(Guid productId);
    }
}
