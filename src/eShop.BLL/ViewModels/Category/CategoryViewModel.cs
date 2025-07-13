using eShop.BLL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace eShop.BLL.ViewModels.Category
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Show in Main Layout")]
        public bool MainLayout { get; set; }

        public int ItemCount { get; set; } 
    }
}

