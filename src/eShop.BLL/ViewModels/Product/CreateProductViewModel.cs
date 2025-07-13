using eShop.BLL.Attributes;
using eShop.DAL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Description { get; set; }
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Product must contain atlest one Image"), Display(Name = "Images")]
        [AllowedListFilesExtensions]
        public List<IFormFile> ProductImages { get; set; }
        [Required, Display(Name = "Price"), PriceComparison(nameof(PrevPrice), "Current Price must be lower than previous Price")]
        public decimal CurrentPrice { get; set; }
        [Display(Name = "previous Price")]
        public decimal? PrevPrice { get; set; }

        [Display(Name = "Product Visibility")]
        public bool Active { get; set; } = true;

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category")]
        public Guid CategoryId { get; set; } = Guid.Empty;

        public SelectList? Categories { get; set; }
    }
}
