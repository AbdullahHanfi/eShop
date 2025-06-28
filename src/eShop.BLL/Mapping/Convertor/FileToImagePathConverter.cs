using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace eShop.BLL.Mapping.Convertor
{
    public class FileToImagePathConverter : ITypeConverter<List<IFormFile>, ICollection<Image>>
    {
        private readonly IImageServices _imageService;

        public FileToImagePathConverter(IImageServices ImageServices)
        {
            _imageService = ImageServices;
        }


        public ICollection<Image> Convert(List<IFormFile> source, ICollection<Image> destination, ResolutionContext context)
        {
            destination = new List<Image>();
            if (source is not null)
                foreach (var file in source)
                {
                    destination.Add(new()
                    {
                        imgPath = _imageService.UploadAsync(file).Result
                    });
                }
            return destination;
        }
    }
}
