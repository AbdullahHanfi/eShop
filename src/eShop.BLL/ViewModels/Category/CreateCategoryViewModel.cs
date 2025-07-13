using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        [Required,
            Display(Name = "Category Name"),
            Remote("UniqueCategoryName", "AdminCategory", "Category", ErrorMessage = "itn't unique category name")]
        public string Name { get; set; }

        [Required, Display(Name = "Join Main Categories")]
        public bool MainLayout { get; set; }
    }
}