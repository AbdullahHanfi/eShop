using System;

namespace eShop.BLL.Exceptions
{
    [Serializable]
    public class NotImageException : Exception
    {
        public string FileName { get; }
        public string FileType { get; }

        public NotImageException() 
            : base("The file is not an image.")
        {
        }

        public NotImageException(string fileName) 
            : base($"The file '{fileName}' is not an image.")
        {
            FileName = fileName;
        }

        public NotImageException(string fileName, string fileType) 
            : base($"The file '{fileName}' is not an image. File type detected: {fileType}")
        {
            FileName = fileName;
            FileType = fileType;
        }

        public NotImageException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}