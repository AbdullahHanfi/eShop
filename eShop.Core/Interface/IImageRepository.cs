using eShop.Core.Entities;

namespace eShop.Core.Interface
{
    public interface IImageRepository : IBaseRepository<Image>
    {
        public Image GetProductImage(Guid productId);
        public Task<Image> GetProductImageAsync(Guid productId);
        public IEnumerable<Image> GetProductImages(Guid productId);
        public Task<IEnumerable<Image>> GetProductImagesAsync(Guid productId);
    }
}
