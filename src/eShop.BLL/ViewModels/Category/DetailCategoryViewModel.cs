using eShop.BLL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace eShop.BLL.ViewModels.Category
{
    public class DetailCategoryViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool MainLayout { get; set; }
        public ICollection<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
