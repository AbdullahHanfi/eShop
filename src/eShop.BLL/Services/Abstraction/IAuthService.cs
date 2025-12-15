
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace eShop.BLL.Services.Abstraction
{
    public interface IAuthService
    {
        Task<ApplicationUser?> GetUserByEmailOrNameAsync(string item);
        Task<(IdentityResult, ApplicationUser?)> RegisterAsync(RegisterViewModel model);
        Task<(ApplicationUser? User, string? Error)> ValidateUserCredentialsAsync(LoginViewModel model);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<bool> SendForgotPasswordEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<(bool Succeeded, string? Path)> UpdateProfilePictureAsync(string userId, IFormFile file);
        Task<bool> IsEmailInUseAsync(string email);
        Task<bool> IsUserNameInUseAsync(string userName);
        Task<bool> SendConfirmationEmailAsync(ApplicationUser user);
    }
}
