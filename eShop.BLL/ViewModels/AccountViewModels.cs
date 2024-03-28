using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels
{
    public class AccountDataViewModel
    {
        [Display(Name = "Email")]
        [ReadOnly(true)]
        public string? Email { get; set; }

        [Display(Name = "UserName")]
        [ReadOnly(true)]
        public string? UserName { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }      
    }   
    public class EditAccountDataViewModel
    {
        public IFormFile? ProfilePicture { get; set; }      
    }   
    public class ComfirmViewModel
    {
        [Required]
        public string userId { get; set; } = string.Empty;
        [Required]
        public string token { get; set; } = string.Empty;
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email/UserName")]
        public string item { get; set; }
    }
    public class LoginViewModel
    {
        [Required, Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
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
    public class ResetPasswordViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string userId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }    

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
