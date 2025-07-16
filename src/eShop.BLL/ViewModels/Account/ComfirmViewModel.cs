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
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
    }

}
