using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email/UserName")]
        public string item { get; set; }
    }

}
