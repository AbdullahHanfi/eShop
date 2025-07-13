using AutoMapper;
using eShop.BLL.ViewModels.Category;
using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NToastNotify;

namespace eShop.MVC.Areas.Category.Controllers
{
    [Route("api")]
    [Area("Category")]
    [ApiController]
    public class UserCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public UserCategoryController(IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            if (_cache.TryGetValue(CacheKeys.ProductsKey, out List<CategoryViewModel> cachedCategories))
            {
                return Ok(cachedCategories);
            }

            var model = await _unitOfWork.Categories.FindAllAsync(e => e.MainLayout);

            if (model is null)
                return Ok(new List<CategoryViewModel>());

            var result = new List<CategoryViewModel>();
            result = _mapper.Map<List<CategoryViewModel>>(model.ToList());

            _cache.Set(CacheKeys.CategoriesKey, result, TimeSpan.FromMinutes(5));

            return Ok(result);
        }
    }
}
