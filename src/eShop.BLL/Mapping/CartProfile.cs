namespace eShop.BLL.Mapping;

using AutoMapper;
using Core.Entities;
using ViewModels.Cart;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartItem,Cart>()
            .ForPath(e=>e.Product.Name,opt=>opt.MapFrom((src=>src.ProductName)))
            .ForPath(e=>e.Product.CurrentPrice,opt=>opt.MapFrom((src=>src.Price)))
            .ForMember(e=>e.CreatedDate,opt=>opt.MapFrom((src=>src.CreatedAt)))
            .ReverseMap();
    }
}