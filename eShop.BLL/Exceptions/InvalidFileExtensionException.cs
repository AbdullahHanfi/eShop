using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Exceptions
{
    public class InvalidFileExtensionException : Exception
    {
        public string InvalidExtension { get; }
        public InvalidFileExtensionException(string extensions) : base($"Invalid file extension. Allowed extensions: {extensions}") { }
        public InvalidFileExtensionException(string extensions, Exception innerException) : base($"Invalid file extension. Allowed extensions: {extensions}", innerException) { }
        public override string ToString()
        {
            string baseString = base.ToString();
            return $"{baseString}. MaxFileSize: {InvalidExtension}";
        }
    }
}
