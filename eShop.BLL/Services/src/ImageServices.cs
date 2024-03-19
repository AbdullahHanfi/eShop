using eShop.BLL.Helper;
using eShop.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Services.src
{
    public class ImageServices : IImageServices
    {
        private readonly PhotoSettings _settings;
        public ImageServices(PhotoSettings settings)
        {
            _settings = settings;
        }
        public async Task<byte[]> Get(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found", path);

            return await File.ReadAllBytesAsync(path);
        }

        public async Task<string> Upload(IFormFile file)
        {

            Directory.CreateDirectory(_settings.DestinationFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_settings.DestinationFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
