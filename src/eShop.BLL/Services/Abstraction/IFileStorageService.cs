using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.Abstraction
{
    public interface IFileStorageService
    {
        void CreateDirectory(string path);
        Task SaveFileAsync(Stream stream, string destinationPath);
    }
}
