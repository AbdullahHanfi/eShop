using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.ViewModels.Account
{
    public class AccountViewModel
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
}
