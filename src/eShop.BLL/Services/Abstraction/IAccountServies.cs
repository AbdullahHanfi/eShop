
using eShop.Core.Entities;

namespace eShop.BLL.Services.Abstraction
{
    public interface IAccountServies
    {
        Task<bool> ConfirmationMail(string? mailTo);
        Task<bool> ResetPasswordToken(string? mailTo);
        Task<ApplicationUser?> UserByEmailOrName(string? item);

    }
}
