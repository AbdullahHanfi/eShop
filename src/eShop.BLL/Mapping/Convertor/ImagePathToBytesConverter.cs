using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;

namespace eShop.BLL.Mapping.Convertor
{
    public class ImagePathToBytesConverter : ITypeConverter<List<Image>, List<byte[]>>
    {
        private readonly IImageServices _imageService;

        public ImagePathToBytesConverter(IImageServices ImageServices)
        {
            _imageService = ImageServices;
        }

        public List<byte[]> Convert(List<Image> source, List<byte[]> destination, ResolutionContext context)
        {
            destination = new();
            if (source is not null)
                foreach (var item in source)
                {
                    var x = _imageService.GetAsync(item.imgPath).Result;
                    if (x is not null)
                        destination.Add(x);
                }

            return destination;
        }
    }
}
