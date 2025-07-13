using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Account
{
    public class ComfirmViewModel
    {
        [Required]
        public string userId { get; set; } = string.Empty;
        [Required]
        public string token { get; set; } = string.Empty;
    }

}
