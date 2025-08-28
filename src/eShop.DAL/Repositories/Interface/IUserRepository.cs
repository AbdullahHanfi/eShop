using eShop.Core.Entities;
using eShop.Core.Utilities;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;

namespace eShop.DAL.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IList<ApplicationUser>> GetAllAsync();
        Task<IList<ApplicationUser>> GetAllAsync(params Expression<Func<ApplicationUser, object>>[] includes);
        Task<IList<ApplicationUser>> GetAllAsync(Expression<Func<ApplicationUser, bool>> criteria);
        Task<IList<ApplicationUser>> GetAllAsync(Expression<Func<ApplicationUser, bool>> criteria, params Expression<Func<ApplicationUser, object>>[] includes);
        Task<IList<ApplicationUser>> GetAllByRoleAsync(string role, params Expression<Func<ApplicationUser, object>>[] includes);
        Task<IList<ApplicationUser>> GetAllByRoleAsync(Guid role, params Expression<Func<ApplicationUser, object>>[] includes);
        
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
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IList<ApplicationRole>> GetRolesAsync(Guid userId);
        Task<bool> IsInRoleAsync(ApplicationUser user, string Role);
        
        Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(ApplicationUser user, string Role);
        Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(Guid userId, string Role);
        Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(Guid userId, Guid Role);

        Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(ApplicationUser user, IEnumerable<string> Roles);
        Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(Guid userId, IEnumerable<string> Roles);

        Task<IdentityOperationResult<ApplicationUser>> RemoveFromRoleAsync(Guid userId, Guid Role);
        Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> Roles);
        Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(Guid userId, IEnumerable<string> Roles);

        Task<IdentityOperationResult<ApplicationUser>> ChangeEmailAsync(ApplicationUser user, string newEmail, string token);
        Task<IdentityOperationResult<ApplicationUser>> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<IdentityOperationResult<ApplicationUser>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        Task<IdentityOperationResult<ApplicationUser>> UpdateAsync(ApplicationUser user);
        Task<IdentityOperationResult<ApplicationUser>> DeleteAsync(ApplicationUser user);
        
        Task<int> CountAsync();
        
    }
}
