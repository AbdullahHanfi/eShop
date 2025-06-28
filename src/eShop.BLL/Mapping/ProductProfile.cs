using AutoMapper;
using eShop.BLL.Mapping.Convertor;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Product;
using eShop.Core.Entities;
using Microsoft.AspNetCore.Http;


namespace eShop.BLL.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<List<Image>, List<byte[]>>().ConvertUsing<ImagePathToBytesConverter>();
            CreateMap<List<IFormFile>, ICollection<Image>>().ConvertUsing<FileToImagePathConverter>();

            CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages))
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.ToList()))
            .ReverseMap();
            
            CreateMap<Product, EditProductViewModel>()
                .ForMember(dest => dest.SendProductImages, opt => opt.MapFrom(src => src.Images.ToList()))
                .ForMember(dest => dest.IDProductImages, opt => opt.MapFrom(src => src.Images.Select(x=>x.Id).ToList()))
                .ReverseMap()
                .ForMember(dest=>dest.Images, opt => opt.MapFrom(src => src.BackProductImages));
        }
    }
}
