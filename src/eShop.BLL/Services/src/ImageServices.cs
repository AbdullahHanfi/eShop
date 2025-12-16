using eShop.BLL.Exceptions;
using eShop.BLL.Services.Abstraction;
using eShop.DAL.Repositories.Interface;
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

            if (!IsValidImageFile(file))
            {
                throw new NotImageException(file.Name);
            }

            _fileStorageService.CreateDirectory(Path.Combine("wwwroot",_photoSettings.DestinationFolder));

            string fileName = _guidProvider.NewGuid().ToString() + extension;
            string filePath = Path.Combine(_photoSettings.DestinationFolder, fileName);

            using (var stream = file.OpenReadStream())
            {
                await _fileStorageService.SaveFileAsync(stream, filePath);
            }

            return filePath;
        }

        private bool IsValidImageFile(IFormFile file)
        {

            var signatures = new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                    }
                },
                { ".jpg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                    }
                },
                { ".png", new List<byte[]>
                    {
                        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                    }
                },
                { ".gif", new List<byte[]>
                    {
                        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, // GIF87a
                        new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }  // GIF89a
                    }
                },
                { ".bmp", new List<byte[]>
                    {
                        new byte[] { 0x42, 0x4D } // BM
                    }
                },
                { ".webp", new List<byte[]>
                    {
                        new byte[] { 0x52, 0x49, 0x46, 0x46 } // RIFF
                    }
                }
            };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!signatures.ContainsKey(extension))
                return false;

            using (var stream = file.OpenReadStream())
            {
                var headerBytes = new byte[8];
                stream.Read(headerBytes, 0, headerBytes.Length);
                stream.Position = 0;

                var validSignatures = signatures[extension];
                foreach (var signature in validSignatures)
                {
                    if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
