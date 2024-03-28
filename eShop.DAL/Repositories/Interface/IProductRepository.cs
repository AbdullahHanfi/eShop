
using eShop.Core.Entities;

namespace eShop.DAL.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Image GetProductImage(Guid productId);
        public Task<Image> GetProductImageAsync(Guid productId);
        public IEnumerable<Image> GetProductImages(Guid productId);
        public Task<IEnumerable<Image>> GetProductImagesAsync(Guid productId);
    }
}
