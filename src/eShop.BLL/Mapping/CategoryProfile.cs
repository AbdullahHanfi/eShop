using AutoMapper;
using eShop.BLL.ViewModels.Category;
using eShop.Core.Entities;

namespace eShop.BLL.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<CreateCategoryViewModel, Category>().ReverseMap();
            CreateMap<EditCategoryViewModel, Category>().ReverseMap();
            CreateMap<DetailCategoryViewModel, Category>().ReverseMap();
        }
    }
}
