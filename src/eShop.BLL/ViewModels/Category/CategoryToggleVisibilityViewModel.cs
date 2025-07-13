using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Category
{
    public class CategoryToggleVisibilityViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool MainLayout { get; set; } 
    }
}
