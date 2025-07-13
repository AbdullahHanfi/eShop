using AutoMapper;
using eShop.BLL.ViewModels.Category;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Caching.Memory;
using NToastNotify;

namespace eShop.MVC.Areas.Category.Controllers
{
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [Area("Category")]
    [Route("admin")]
    public class AdminCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;


        public AdminCategoryController(IMapper mapper, IUnitOfWork unitOfWork, IToastNotification toastNotification, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> Index()
        {
            var model = await _unitOfWork.Categories.GetAllAsync();

            var result = new List<CategoryViewModel>();
            if (model is not null)
            {
                result = _mapper.Map<List<CategoryViewModel>>(model.ToList());
                result.ForEach(category => category.ItemCount = _unitOfWork.Products.Count(product => product.CategoryId == category.Id));
            }

            return View(result);
        }


        [HttpGet("Category/create")]
        public async Task<IActionResult> Add() => View(new CreateCategoryViewModel());

        [HttpPost("Category/create")]
        public async Task<IActionResult> Add(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = _mapper.Map<Core.Entities.Category>(model);

            try
            {
                await _unitOfWork.Categories.AddAsync(category);
                _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Category added✅");

                _cache.Remove(CacheKeys.CategoriesKey); // Clear cache after adding a new category
            }
            catch (Exception)
            {
                _toastNotification.AddWarningToastMessage("Error: problem in database!");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("category/detail")]
        public async Task<IActionResult> Detail(Guid Id)
        {
            if (Id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var category = await _unitOfWork.Categories.FindAsync(e => e.Id == Id, e => e.Products);


            if (category == null)
            {
                _toastNotification.AddWarningToastMessage("category not found!");
                return RedirectToAction(nameof(Index));
            }

            foreach (var product in category.Products)
            {
                product.Images = [await _unitOfWork.Products.GetProductImageAsync(product.Id)];
            }

            return View(_mapper.Map<DetailCategoryViewModel>(category));
        }

        [HttpGet("category/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Index));
            var category = _unitOfWork.Categories.GetById(id);

            if (category is null)
            {
                _toastNotification.AddWarningToastMessage("category not found!");
                return RedirectToAction(nameof(Index));
            }
            category = await _unitOfWork.Categories.FindAsync(e => e.Id == id, e => e.Products);

            return View(_mapper.Map<EditCategoryViewModel>(category));
        }

        [HttpPost("category/edit")]
        public IActionResult Edit(EditCategoryViewModel category)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (category.Id == Guid.Empty)
            {
                _toastNotification.AddWarningToastMessage("Error uploading category try again!");
                return RedirectToAction(nameof(Index));
            }

            var model = _mapper.Map<Core.Entities.Category>(category);

            try
            {
                _unitOfWork.Categories.Update(model);
                _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Category modified✅");

                _cache.Remove(CacheKeys.CategoriesKey); // Clear cache after updating a category
            }
            catch (Exception)
            {
                _toastNotification.AddWarningToastMessage("Error: problem in database!");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("unique-category-name")]
        public async Task<IActionResult> UniqueCategoryName(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return Json("Name is required");
            }
            var category = await _unitOfWork.Categories.FindAsync(e => e.Name == name);

            if (category is not null)
            {
                return Json($"This category name is already in use");
            }

            return Json(true);
        }

        [HttpPost("toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility([FromForm] CategoryToggleVisibilityViewModel model)
        {

            if (model.Id == Guid.Empty)
            {
                return BadRequest("Category ID is required.");
            }
            var category = await _unitOfWork.Categories.GetByIdAsync(model.Id);

            if (category is null || category.IsDeleted)
            {
                return NotFound($"No category found for ID {model.Id}.");
            }

            category.MainLayout = model.MainLayout;
            _unitOfWork.Categories.Update(category);

            try
            {
                _unitOfWork.Complete();

                _cache.Remove(CacheKeys.CategoriesKey); // Clear cache after toggling visibility
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the category.");
            }

            return NoContent();
        }

        [HttpDelete("category/delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Category ID is required.");
            }
            var category = _unitOfWork.Categories.GetById(Id);

            if (category is null || category.IsDeleted)
            {
                return NotFound($"No category found for ID {Id}.");
            }

            category.IsDeleted = true;
            _unitOfWork.Categories.Update(category);

            try
            {
                _unitOfWork.Complete();
                _cache.Remove(CacheKeys.CategoriesKey); // Clear cache after Deleting a category
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the category.");
            }
            _toastNotification.AddWarningToastMessage("good");

            return NoContent();
        }

        [HttpPost("category/delete")]
        public async Task<IActionResult> Delete(List<Guid> ids)
        {

            if (ids.Count == 0 || ids[0] == Guid.Empty)
            {
                return BadRequest("Category ID is required.");
            }

            var categories = await _unitOfWork.Categories.FindAllAsync(e => ids.Contains(e.Id));

            if (categories is null)
            {
                return NotFound($"No category found for Ids.");
            }

            try
            {
                foreach (var category in categories)
                {
                    category.IsDeleted = true;
                    _unitOfWork.Categories.Update(category);
                }
                _unitOfWork.Complete();

                _cache.Remove(CacheKeys.CategoriesKey); // Clear cache after Deleting a categories
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the category.");
            }

            return NoContent();
        }

    }
}
