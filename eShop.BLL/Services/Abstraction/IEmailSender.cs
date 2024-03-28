using Microsoft.AspNetCore.Http;

namespace eShop.BLL.Services.Abstraction
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string mailto, string subject, string body);
        Task SendEmailAsync(string mailto, string subject, string body, List<IFormFile> files);
    }
}
