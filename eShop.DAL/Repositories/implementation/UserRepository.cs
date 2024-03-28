using eShop.Core.Entities;
using eShop.DAL.Interface;
using eShop.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.DAL.Repositories.implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(UserManager<ApplicationUser> userManager) => _userManager = userManager;


        public async Task<ApplicationUser?> FindByIdAsync(string userId)
            => await _userManager.FindByIdAsync(userId);

        public async Task<ApplicationUser?> FindByEmailAsync(string Email)
            => await _userManager.FindByEmailAsync(Email);

        public async Task<ApplicationUser?> FindByNameAsync(string userName)
            => await _userManager.FindByNameAsync(userName);
        public async Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal user)
            => await _userManager.GetUserAsync(user);

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
            => await _userManager.IsEmailConfirmedAsync(user);

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
            => await _userManager.GenerateEmailConfirmationTokenAsync(user);
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityOperationResult<ApplicationUser>> CreateAsync(ApplicationUser user)
            => await ConvertIdentityToCustome(await _userManager.CreateAsync(user));

        public async Task<IdentityOperationResult<ApplicationUser>> CreateAsync(ApplicationUser user, string password)
            => await ConvertIdentityToCustome(await _userManager.CreateAsync(user, password));

        public async Task<IdentityOperationResult<ApplicationUser>> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => await ConvertIdentityToCustome(await _userManager.ResetPasswordAsync(user, token, newPassword));

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(ApplicationUser user, string Role)
            => await ConvertIdentityToCustome(await _userManager.AddToRoleAsync(user, Role));

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(ApplicationUser user, IEnumerable<string> Roles)
            => await ConvertIdentityToCustome(await _userManager.AddToRolesAsync(user, Roles));

        public async Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> Roles)
            => await ConvertIdentityToCustome(await _userManager.RemoveFromRolesAsync(user, Roles));

        public async Task<IdentityOperationResult<ApplicationUser>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
            => await ConvertIdentityToCustome(await _userManager.ChangePasswordAsync(user, currentPassword, newPassword));

        public async Task<IdentityOperationResult<ApplicationUser>> ChangeEmailAsync(ApplicationUser user, string newEmail, string token)
            => await ConvertIdentityToCustome(await _userManager.ChangeEmailAsync(user, newEmail, token));

        public async Task<IdentityOperationResult<ApplicationUser>> ConfirmEmailAsync(ApplicationUser user, string token)
            => await ConvertIdentityToCustome(await _userManager.ConfirmEmailAsync(user, token));

        public async Task<IdentityOperationResult<ApplicationUser>> UpdateAsync(ApplicationUser user)
            => await ConvertIdentityToCustome(await _userManager.UpdateAsync(user));

        public async Task<IdentityOperationResult<ApplicationUser>> DeleteAsync(ApplicationUser user)
            => await ConvertIdentityToCustome(await _userManager.DeleteAsync(user));

        private async Task<IdentityOperationResult<ApplicationUser>> ConvertIdentityToCustome(IdentityResult identityResult)
        {
            var result = new IdentityOperationResult<ApplicationUser>
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(error => error.Description)
            };

            return result;
        }

    }
}
