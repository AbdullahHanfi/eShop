using eShop.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.DAL.Repositories.implementation
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
