using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.Abstraction
{
    public interface IImageServices
    {
        Task<string> Upload(IFormFile image);
        Task<byte[]> Get(string Path);
    }
}
