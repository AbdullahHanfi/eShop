using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, EmailAddress, Display(Name = "Email")]
        [Remote(action: "IsAlreadySigned", controller: "Account", areaName: "Auth", HttpMethod = "POST", ErrorMessage = "Email is already exists.")]
        public string? Email { get; set; }

        [Required, Display(Name = "UserName")]
        [Remote(action: "IsUsedName", controller: "Account", areaName: "Auth", HttpMethod = "POST", ErrorMessage = "User Name is already exists.")]
        public string? UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
