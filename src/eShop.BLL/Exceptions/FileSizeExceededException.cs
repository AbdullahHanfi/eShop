using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Exceptions
{
    public class FileSizeExceededException : Exception
    {
        public long MaxFileSize { get; }
        public FileSizeExceededException(long maxFileSize) : base($"File size exceeds the maximum allowed ({maxFileSize} bytes).")
        {
            MaxFileSize = maxFileSize;
        }

        public FileSizeExceededException(long maxFileSize, Exception innerException) : base($"File size exceeds the maximum allowed ({maxFileSize} bytes).", innerException)
        {
            MaxFileSize = maxFileSize;
        }
        public override string ToString()
        {
            string baseString = base.ToString();
            return $"{baseString}. MaxFileSize: {MaxFileSize}";
        }
    }
}
