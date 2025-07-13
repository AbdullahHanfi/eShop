using eShop.BLL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Category
{
    public class EditCategoryViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        [Required, Display(Name = "Category Name")]
        public string Name { get; set; }
        [Required, Display(Name = "Join Main Categories")]
        public bool MainLayout { get; set; }
    }
}
