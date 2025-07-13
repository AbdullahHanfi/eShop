namespace eShop.DAL.Repositories.Interface
{
    public interface IFileStorageService
    {
        void CreateDirectory(string path);
        Task SaveFileAsync(Stream stream, string destinationPath);
    }
}
