using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;
using eShop.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace eShop.BLL.Services.src
{
    public class AccountServies : IAccountServies
    {
        private readonly IEmailSender _mailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        public AccountServies(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory, IUnitOfWork UnitOfWork, IHttpContextAccessor httpContextAccessor, IEmailSender mailServies)
        {
            _UnitOfWork = UnitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mailSender = mailServies;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;

        }
        public async Task<bool> ConfirmationMail(string? mailTo)
        {
            var user = await UserByEmailOrName(mailTo);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            if (user is not null)
            {
                string code = await _UnitOfWork.Users.GenerateEmailConfirmationTokenAsync(user);
                string BaseUrl = string.Format("{0}://{1}", _httpContextAccessor.HttpContext.Request.Scheme.ToString(), _httpContextAccessor.HttpContext.Request.Host.ToString());
                string url = urlHelper.Action("ConfirmEmail", new { userId = user.Id, token = code });

                await _mailSender.SendEmailAsync(user.Email, "Confirm your account.", $"Please use the link to confirm your email account <a href='{BaseUrl}{url}'> Click.</a>");
                return true;
            }
            return false;
        }

        public async Task<ApplicationUser?> UserByEmailOrName(string? item)
        {
            if (string.IsNullOrEmpty(item))
                return default;
            ApplicationUser user;
            var nameOrEmail = item.Any(b => b == '@');
            if (nameOrEmail)
                user = await _UnitOfWork.Users.FindByEmailAsync(item);
            else
                user = await _UnitOfWork.Users.FindByNameAsync(item);
            return user;
        }

        public async Task<bool> ResetPasswordToken(string? mailTo)
        {
            var user = await UserByEmailOrName(mailTo);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            if (user is not null)
            {
                string code = await _UnitOfWork.Users.GeneratePasswordResetTokenAsync(user);
                string BaseUrl = string.Format("{0}://{1}", _httpContextAccessor.HttpContext.Request.Scheme.ToString(), _httpContextAccessor.HttpContext.Request.Host.ToString());
                string url = urlHelper.Action("ResetPassword", new { userId = user.Id, token = code });

                await _mailSender.SendEmailAsync(user.Email, "Reset Password.", $"Please use the link to Reset your Password account <a href='{BaseUrl}{url}'> Click.</a>");
                return true;
            }
            return false;
        }
    }
}
