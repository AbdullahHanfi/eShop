using AutoMapper;
using eShop.BLL.ViewModels.Product;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            int productCount = _unitOfWork.Products.Count();
            var products = _unitOfWork.Products.Take(Math.Min(5, productCount));
            if (products.Count() != 0)
            {
                foreach (var product in products)
                {
                    product.Images = [.. await _unitOfWork.Products.GetProductImagesAsync(product.Id)];
                }
                foreach (var product in products)
                {
                    ViewData[product.Name] = _unitOfWork.Products.Find(e => e.Id == product.Id, e => e.Category).Category.Name;
                }

                return View(_mapper.Map<List<ProductViewModel>>(products));
            }
            return View(new List<ProductViewModel>());
        }

        public IActionResult About() => View();

        public IActionResult Privacy() => View();

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                500 => View("ServerError"),
                404 => View("NotFound"),
                403 => View("AccessDenied"),
                401 => View("Unauthorized"),
                _ => View("Error"),
            };
        }
    }
}