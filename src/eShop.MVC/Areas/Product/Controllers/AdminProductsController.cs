using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Product;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace eShop.MVC.Areas.Product.Controllers
{
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [Area("product")]
    [Route("admin")]
    public class AdminProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly IImageServices _imageService;
        private readonly IMapper _mapper;

        public AdminProductsController(IMapper mapper, IUnitOfWork unitOfWork, IToastNotification toastNotification, IImageServices imageServices)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _imageService = imageServices;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = (page.HasValue && page > 0) ? Convert.ToInt32(page) : 1;
            ViewBag.TotalItems = _unitOfWork.Products.Count();
            ViewBag.Pages = (ViewBag.TotalItems + Page.MaxElementsInPage - 1) / Page.MaxElementsInPage;
            ViewBag.PageNumber = pageNumber;

            var model = _unitOfWork.Products.Include("Images").Skip((pageNumber - 1) * Page.MaxElementsInPage).Take(Page.MaxElementsInPage).ToList();

            var result = _mapper.Map<List<ProductViewModel>>(model);

            return View(result);
        }


        [HttpGet("product/add")]
        public async Task<IActionResult> Add()
        {
            var model = new CreateProductViewModel()
            {
                Categories =
                new SelectList((
                await _unitOfWork.Categories.GetAllAsync())
                .ToList()
                , "Id"
                , "Name")
            };
            return View(model);
        }

        [HttpPost("product/add")]
        public async Task<IActionResult> Add(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = _mapper.Map<Core.Entities.Product>(model);

            try
            {
                await _unitOfWork.Products.AddAsync(product);
                _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Product added✅");
            }
            catch (Exception)
            {
                _toastNotification.AddWarningToastMessage("Error: problem in database!");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("product/detail")]
        public async Task<IActionResult> Detail(Guid Id)
        {
            if (Id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var product = _unitOfWork.Products.GetById(Id);
            product.Images = _unitOfWork.Products.GetProductImages(Id).ToList();

            if (product == null)
            {
                _toastNotification.AddWarningToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [HttpGet("product/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Index));
            var product = await _unitOfWork.Products.FindAsync(e => e.Id == id, e => e.Images);

            if (product is null)
            {
                _toastNotification.AddWarningToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }

            var model = _mapper.Map<EditProductViewModel>(product);

            model.Categories = new SelectList((
                await _unitOfWork.Categories.GetAllAsync())
                .ToList()
                , "Id"
                , "Name");

            return View(model);
        }

        [HttpPost("product/edit")]
        public async Task<IActionResult> Edit(EditProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                product.SendProductImages = _mapper.Map<List<byte[]>>(_unitOfWork.Products.GetProductImages(product.Id).ToList());
                return View(product);
            }

            if (product.Id == Guid.Empty)
            {
                _toastNotification.AddWarningToastMessage("Error uploading product try again!");
                return RedirectToAction(nameof(Index));
            }

            var model = _mapper.Map<Core.Entities.Product>(product);
            if (!model.Images.Any() && product.IsDeletedProductImages.Count(e => !e) == 0)
            {
                _toastNotification.AddWarningToastMessage("Plz upload alreast one image");
                return RedirectToAction(nameof(Index));
            }

            for (int i = 0; i < product.IDProductImages.Count(); i++)
            {
                if (product.IsDeletedProductImages[i])
                {
                    var image = _unitOfWork.Images.GetById(product.IDProductImages[i]);
                    image.IsDeleted = true;
                    _unitOfWork.Images.Update(image);
                }
            }
            _unitOfWork.Products.Update(model);
            try
            {
                _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Product modified✅");
            }
            catch (Exception)
            {
                _toastNotification.AddWarningToastMessage("Error: problem in database!");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("product/delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Product ID is required.");
            }
            var product = _unitOfWork.Products.GetById(Id);

            if (product is null || product.IsDeleted)
            {
                return NotFound($"No product found for ID {Id}.");
            }

            product.IsDeleted = true;
            _unitOfWork.Products.Update(product);

            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }

            return NoContent();
        }

    }
}
