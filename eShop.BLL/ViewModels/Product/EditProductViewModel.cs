using eShop.BLL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.ViewModels.Product
{
    public class EditProductViewModel
    {
        [Required, HiddenInput]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }

        public List<byte[]>? SendProductImages { get; set; }
        [HiddenInput]
        public List<Guid> IDProductImages { get; set; }
        [HiddenInput]
        public List<bool> IsDeletedProductImages { get; set; }
        public List<IFormFile>? BackProductImages { get; set; }
        
        [Required, Display(Name = "Price"), PriceComparison(nameof(PrevPrice), "Current Price must be lower than previous Price")]
        public decimal CurrentPrice { get; set; }
        [Display(Name = "previous Price")]
        public decimal? PrevPrice { get; set; }
    }
}
