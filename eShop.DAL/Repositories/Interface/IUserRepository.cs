using eShop.Core.Entities;
using eShop.Core.Utilities;
using System.Security.Claims;

namespace eShop.DAL.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<ApplicationUser?> FindByEmailAsync(string Email);
        Task<ApplicationUser?> FindByNameAsync(string userName);
        Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal principal);
        Task<IdentityOperationResult<ApplicationUser>> CreateAsync(ApplicationUser user);
        Task<IdentityOperationResult<ApplicationUser>> CreateAsync(ApplicationUser user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityOperationResult<ApplicationUser>> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(ApplicationUser user, string Role);
        Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(ApplicationUser user, IEnumerable<string> Roles);
        Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> Roles);
        Task<IdentityOperationResult<ApplicationUser>> ChangeEmailAsync(ApplicationUser user, string newEmail, string token);
        Task<IdentityOperationResult<ApplicationUser>> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<IdentityOperationResult<ApplicationUser>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<IdentityOperationResult<ApplicationUser>> UpdateAsync(ApplicationUser user);
        Task<IdentityOperationResult<ApplicationUser>> DeleteAsync(ApplicationUser user);
    }
}
