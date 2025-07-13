using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.ViewModels.Account
{
    public class EditAccountDataViewModel
    {
        public IFormFile? ProfilePicture { get; set; }
    }
}
