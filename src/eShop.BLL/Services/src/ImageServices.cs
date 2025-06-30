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
        private readonly IGuidProvider _guidProvider;
        private readonly IFileStorageService _fileStorageService;

        public ImageServices(IOptions<PhotoSettings> settings, IGuidProvider guidProvider, IFileStorageService fileStorageService)
        {
            _photoSettings = settings.Value;
            _guidProvider = guidProvider;
            _fileStorageService = fileStorageService;
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

            _fileStorageService.CreateDirectory(_photoSettings.DestinationFolder);

            string fileName = _guidProvider.NewGuid().ToString() + extension;
            string filePath = Path.Combine(_photoSettings.DestinationFolder, fileName);

            using (var stream = file.OpenReadStream())
            {
                await _fileStorageService.SaveFileAsync(stream, filePath);
            }
            
            return filePath;
        }
    }
}
