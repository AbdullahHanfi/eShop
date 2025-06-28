using eShop.BLL.Exceptions;
using eShop.BLL.Services.Abstraction;
using eShop.DAL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace eShop.BLL.Services.src
{
    public class ImageServices : IImageServices
    {
        private readonly PhotoSettings _photoSettings;
        public ImageServices(IOptions<PhotoSettings> settings)
        {
            _photoSettings = settings.Value;
        }
        public async Task<byte[]> GetAsync(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found", path);

            return await File.ReadAllBytesAsync(path);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            if (file.Length > _photoSettings.MaxFileSizeBytes)
            {
                throw new FileSizeExceededException(_photoSettings.MaxFileSizeBytes);
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_photoSettings.AllowedExtensions.Contains(extension))
            {
                throw new InvalidFileExtensionException(_photoSettings.AllowedExtensions.Aggregate((x, z) => x + ", " + z));
            }

            Directory.CreateDirectory(_photoSettings.DestinationFolder);

            string fileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(_photoSettings.DestinationFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

    }
}
