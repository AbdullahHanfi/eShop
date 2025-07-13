using eShop.DAL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.Attributes
{
    public class AllowedListFilesExtensionsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var photoSettings = validationContext.GetRequiredService<IOptions<PhotoSettings>>().Value;

            string _fileExtension = photoSettings.AllowedExtensions.Aggregate((x, z) => x + ", " + z);
            ErrorMessage = $"Please upload a valid image file ({_fileExtension})";

            if (value is null)
                return new ValidationResult("Please select a valid files");

            if (value is not List<IFormFile> files)
                return new ValidationResult("Please select a valid files");

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_fileExtension.Contains(extension))
                {
                    return new ValidationResult("Please select a valid files");
                }
            }

            return ValidationResult.Success;
        }
    }
}
