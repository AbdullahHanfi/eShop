
using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace eShop.BLL.Services.src
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageServices _imageService;
        private readonly IEmailSender _mailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IImageServices imageService,
            IEmailSender mailSender,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _imageService = imageService;
            _mailSender = mailSender;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<ApplicationUser?> GetUserByEmailOrNameAsync(string item)
        {
            if (string.IsNullOrEmpty(item))
                return null;

            return item.Contains('@')
                ? await _userManager.FindByEmailAsync(item)
                : await _userManager.FindByNameAsync(item);
        }

        public async Task<(IdentityResult, ApplicationUser?)> RegisterAsync(RegisterViewModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.EmailConfirmed = true; // TODO: This should be false and rely on email confirmation
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);
            }
            return (result, user);
        }

        public async Task<(ApplicationUser? User, string? Error)> ValidateUserCredentialsAsync(LoginViewModel model)
        {
            var user = await GetUserByEmailOrNameAsync(model.Email);
            if (user == null)
            {
                return (null, "Invalid login attempt");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return (null, "Confirm your Email first.");
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return (null, "Invalid login attempt");
            }

            return (user, null);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<bool> SendForgotPasswordEmailAsync(string email)
        {
            var user = await GetUserByEmailOrNameAsync(email);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            string callbackUrl = urlHelper.Action("ResetPassword", "Auth",
                new { area = "Auth", userId = user.Id, token = token },
                _httpContextAccessor.HttpContext.Request.Scheme);

            await _mailSender.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");
            return true;
        }

        public async Task<bool> SendConfirmationEmailAsync(ApplicationUser user)
        {
            if (user == null) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            string callbackUrl = urlHelper.Action("ConfirmEmail", "Auth",
                new { area = "Auth", userId = user.Id, token = token },
                _httpContextAccessor.HttpContext.Request.Scheme);

            await _mailSender.SendEmailAsync(user.Email, "Confirm your account.", $"Please use the link to confirm your email account <a href='{callbackUrl}'> Click.</a>");
            return true;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);
            if (user == null || user.Email != model.Email)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid user." });
            }
            return await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        }

        public async Task<(bool Succeeded, string? Path)> UpdateProfilePictureAsync(string userId, IFormFile? file)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || file == null)
            {
                return (false, null);
            }

            try
            {
                var imagePath = await _imageService.UploadAsync(file);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    user.imgPath = imagePath;
                    await _userManager.UpdateAsync(user);
                    return (true, imagePath);
                }
                return (false, null);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var user = await GetUserByEmailOrNameAsync(email);
            return user != null;
        }

        public async Task<bool> IsUserNameInUseAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return false;
            var user = await GetUserByEmailOrNameAsync(userName);
            return user != null;
        }
    }
}
