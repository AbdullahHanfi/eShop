using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Product;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace eShop.MVC.Areas.Product.Controllers {
    [Area("Product")]
    [Route("customer/products")]
    [Authorize("GuestOrCustomer")]
    public class CustomerProductsController : Controller {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly IImageServices _imageService;
        private readonly IMapper _mapper;

        public CustomerProductsController(IMapper mapper, IUnitOfWork unitOfWork, IToastNotification toastNotification, IImageServices imageServices) {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _imageService = imageServices;
            _mapper = mapper;
        }

        [HttpGet("/products")]
        public IActionResult Index(ProductSearchResultViewModel? searchResult, int? page) {
            int pageNumber = (page.HasValue && page > 0) ? Convert.ToInt32(page) : 1;
            ViewBag.TotalItems = _unitOfWork.Products.Count();
            ViewBag.Pages = (ViewBag.TotalItems + Page.MaxElementsInPage - 1) / Page.MaxElementsInPage;
            ViewBag.PageNumber = pageNumber;

            var model = _mapper.Map<List<ProductViewModel>>(
            _unitOfWork.Products.FindAll(e => e.Active, i => i.Images)
                .Skip((pageNumber - 1) * Page.MaxElementsInPage)
                .Take(Page.MaxElementsInPage)
                .ToList());

            ApplySorting(model, searchResult?.SortBy, searchResult?.SortOrder);

            var result = new ProductSearchResultViewModel()
            {
                Products = model,
                SearchTerm = searchResult?.SearchTerm ?? string.Empty,
                TotalItems = ViewBag.TotalItems,
                CurrentPage = pageNumber,
                TotalPages = ViewBag.Pages,
                SortBy = searchResult?.SortBy ?? "name",
                SortOrder = searchResult?.SortOrder ?? "asc",
                PageSize = Page.MaxElementsInPage
            };

            return View(result);
        }

        [HttpGet("/product/detail/{Id}")]
        public async Task<IActionResult> Detail(Guid Id) {
            if (Id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var product = await _unitOfWork.Products.FindAsync(e => e.Id == Id && e.Active, i => i.Images);

            if (product == null){
                _toastNotification.AddWarningToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [HttpGet("/products/category/{Id}")]
        public async Task<IActionResult> ProductsByCategory(Guid Id) {
            if (Id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var category = await _unitOfWork.Categories.FindAsync(e => e.Id == Id);

            if (category is null){
                _toastNotification.AddWarningToastMessage("Category not found!");
                return RedirectToAction(nameof(Index));
            }

            var products = await _unitOfWork.Products.FindAllAsync(e => e.Category.Id == Id && e.Active, i => i.Images);

            if (products == null){
                _toastNotification.AddWarningToastMessage("Products not found!");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryName = category.Name;

            return View(_mapper.Map<List<ProductViewModel>>(products.ToList()));
        }

        [HttpGet("/products/search")]
        public async Task<IActionResult> Search(string searchString, int? page, string sortBy = "name", string sortOrder = "asc") {
            if (string.IsNullOrWhiteSpace(searchString)){
                _toastNotification.AddWarningToastMessage("Please enter a search term.");
                return RedirectToAction(nameof(Index));
            }

            // Sanitize and prepare search parameters
            searchString = searchString.Trim();
            int pageNumber = page ?? 1;
            int pageSize = Page.MaxElementsInPage;

            // Store search parameters for view
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.PageNumber = pageNumber;

            var matchProducts = await _unitOfWork.Products
                .FindAllAsync(p => p.Name.ToLower().Contains(searchString.ToLower()) ||
                                   p.Description.ToLower().Contains(searchString.ToLower()),
                e => e.Images);

            var query = _mapper.Map<List<ProductViewModel>>(matchProducts.ToList());


            var totalItems = query.Count();

            if (totalItems == 0){
                _toastNotification.AddInfoToastMessage($"No products found for '{searchString}'");
                ViewBag.SearchPerformed = true;
                return View(nameof(Index));
            }

            query = ApplySorting(query, sortBy, sortOrder);

            var products = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new ProductSearchResultViewModel
            {
                Products = products,
                SearchTerm = searchString,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            ViewBag.SearchPerformed = true;
            _toastNotification.AddSuccessToastMessage($"Found {totalItems} products matching '{searchString}'");

            return View(nameof(Index), result);
        }
        [HttpGet("/api/products/image/{ProductId}")]
        public async Task<IActionResult> SearchSuggestions(Guid ProductId) {
            var img = await _unitOfWork.Products.GetProductImageAsync(ProductId);
            Response.Headers.Add("Cache-Control", "public, max-age=86400"); // Cache for 1 day
            return File(await _imageService.GetAsync(img.imgPath), "image/jpeg");
        }
        [HttpGet("/search-suggestions")]
        public IActionResult SearchSuggestions(string term) {
            if (string.IsNullOrWhiteSpace(term) || term.Length <= 1)
                return Json(new List<string>());

            var suggestions = _unitOfWork.Products
                .FindAll(p => p.Active && p.Name.ToLower().Contains(term.ToLower()))
                .Select(p => p.Name)
                .Distinct()
                .Take(10)
                .ToList();

            return Json(suggestions);
        }

        private List<ProductViewModel> ApplySorting(List<ProductViewModel> query, string sortBy, string sortOrder) {
            var isDescending = sortOrder?.ToLower() == "desc";

            return sortBy?.ToLower() switch
            {
                "price" => isDescending ? query.OrderByDescending(p => p.CurrentPrice).ToList() : query.OrderBy(p => p.CurrentPrice).ToList(),
                "rating" => isDescending ? query.OrderByDescending(p => p.Rate).ToList() : query.OrderBy(p => p.Rate).ToList(),
                _ => isDescending ? query.OrderByDescending(p => p.Name).ToList() : query.OrderBy(p => p.Name).ToList()
            };
        }
    }
}