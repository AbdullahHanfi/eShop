using Microsoft.AspNetCore.Http;

namespace eShop.Core.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string mailto, string subject, string body);
        Task SendEmailAsync(string mailto, string subject, string body, List<IFormFile> files);
    }
}
