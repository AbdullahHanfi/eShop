using eShop.BLL.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.src
{
    public class FileStorageService : IFileStorageService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public async Task SaveFileAsync(Stream stream, string destinationPath)
        {
            using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            {
               await stream.CopyToAsync(fileStream);
            }
        }
    }
}
