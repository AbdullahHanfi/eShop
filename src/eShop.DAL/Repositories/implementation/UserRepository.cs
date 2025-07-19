using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL.Data;
using eShop.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace eShop.DAL.Repositories.implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly eShopDbContext _context;

        public UserRepository(eShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
            => ConvertIdentityToCustome(await _userManager.CreateAsync(user));

        public async Task<IdentityOperationResult<ApplicationUser>> CreateAsync(ApplicationUser user, string password)
            => ConvertIdentityToCustome(await _userManager.CreateAsync(user, password));

        public async Task<IdentityOperationResult<ApplicationUser>> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => ConvertIdentityToCustome(await _userManager.ResetPasswordAsync(user, token, newPassword));

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(ApplicationUser user, string Role)
            => ConvertIdentityToCustome(await _userManager.AddToRoleAsync(user, Role));

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(ApplicationUser user, IEnumerable<string> Roles)
            => ConvertIdentityToCustome(await _userManager.AddToRolesAsync(user, Roles));

        public async Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> Roles)
            => ConvertIdentityToCustome(await _userManager.RemoveFromRolesAsync(user, Roles));

        public async Task<IdentityOperationResult<ApplicationUser>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
            => ConvertIdentityToCustome(await _userManager.ChangePasswordAsync(user, currentPassword, newPassword));

        public async Task<IdentityOperationResult<ApplicationUser>> ChangeEmailAsync(ApplicationUser user, string newEmail, string token)
            => ConvertIdentityToCustome(await _userManager.ChangeEmailAsync(user, newEmail, token));

        public async Task<IdentityOperationResult<ApplicationUser>> ConfirmEmailAsync(ApplicationUser user, string token)
            => ConvertIdentityToCustome(await _userManager.ConfirmEmailAsync(user, token));

        public async Task<IdentityOperationResult<ApplicationUser>> UpdateAsync(ApplicationUser user)
            => ConvertIdentityToCustome(await _userManager.UpdateAsync(user));

        public async Task<IdentityOperationResult<ApplicationUser>> DeleteAsync(ApplicationUser user)
            => ConvertIdentityToCustome(await _userManager.DeleteAsync(user));

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => await _userManager.GetRolesAsync(user);

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string Role)
            => await _userManager.IsInRoleAsync(user, Role);

        private IdentityOperationResult<ApplicationUser> ConvertIdentityToCustome(IdentityResult identityResult)
        {

            var result = new IdentityOperationResult<ApplicationUser>
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(error => error.Description)
            };

            return result;
        }

        public async Task<IList<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IList<ApplicationUser>> GetAllAsync(Expression<Func<ApplicationUser, bool>> criteria)
        {
            return await _context.Users
                .Where(criteria)
                .ToListAsync();
        }

        public async Task<IList<ApplicationUser>> GetAllAsync(Expression<Func<ApplicationUser, bool>> criteria, params Expression<Func<ApplicationUser, object>>[] includes)
        {
            IQueryable<ApplicationUser> query = _context.Users.Where(criteria);
            if (includes != null && includes.Length != 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRoleAsync(Guid userId, string Role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new IdentityOperationResult<ApplicationUser>
                {
                    Succeeded = false,
                    Errors = ["User not found."]
                };
            }
            var result = await _userManager.AddToRoleAsync(user, Role);
            return ConvertIdentityToCustome(result);
        }

        public async Task<IdentityOperationResult<ApplicationUser>> AddToRolesAsync(Guid userId, IEnumerable<string> Roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new IdentityOperationResult<ApplicationUser>
                {
                    Succeeded = false,
                    Errors = ["User not found."]
                };
            }
            var result = await _userManager.AddToRolesAsync(user, Roles);
            return ConvertIdentityToCustome(result);
        }

        public async Task<IdentityOperationResult<ApplicationUser>> RemoveFromRolesAsync(Guid userId, IEnumerable<string> Roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new IdentityOperationResult<ApplicationUser>
                {
                    Succeeded = false,
                    Errors = ["User not found."]
                };
            }
            var result = await _userManager.RemoveFromRolesAsync(user, Roles);
            return ConvertIdentityToCustome(result);
        }

        public async Task<IList<ApplicationUser>> GetAllAsync(params Expression<Func<ApplicationUser, object>>[] includes)
        {
            IQueryable<ApplicationUser> query = _context.Users;
            if (includes != null && includes.Length != 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IList<ApplicationUser>> GetAllByRoleAsync(string role, params Expression<Func<ApplicationUser, object>>[] includes)
        {
            role = role.ToUpper();
            IQueryable<ApplicationUser> query = _context.Users.FromSqlRaw(@"
            SELECT *
            FROM AspNetUsers
            where Id in (SELECT [UserId]
                        FROM [AspNetUserRoles]
                        where [RoleId] in (SELECT [Id]
                                            FROM [AspNetRoles]
                                            where [NormalizedName] = 'CUSTOMER'))"
            , new SqlParameter("@role", role));
            
            if (includes != null && includes.Length != 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }
    }
}
