using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;
using eShop.DAL.Interface;
using eShop.DAL.Utilities;
using eShop.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;

namespace eShop.MVC.Areas.Auth.Controllers
{

    [Area("Auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountServies _accountServies;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly PhotoSettings _photoSettings;
        private readonly IImageServices _imageService;

        public AuthController(IImageServices ImageServices, IOptions<PhotoSettings> PhotoSettings, IUnitOfWork UnitOfWork, IMapper mapper, SignInManager<ApplicationUser> signInManager, IAccountServies accountServies, IToastNotification toastNotification)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _accountServies = accountServies;
            _toastNotification = toastNotification;
            _photoSettings = PhotoSettings.Value;
            _imageService = ImageServices;
        }

        [Authorize]
        [Route("Account")]
        [RedirectIfNotAuthenticated]
        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.Users.GetUserAsync(User);

            if (user is null)
                return RedirectToAction("Index", "Home");

            ViewBag.accept = _photoSettings.AllowedExtensions.Aggregate((x, z) => x + ", " + z);

            var model = _mapper.Map<AccountDataViewModel>(user);

            if (string.IsNullOrEmpty(user.imgPath) is false)
                model.ProfilePicture = await _imageService.GetAsync(user.imgPath);

            return View(model);
        }

        [HttpPost("Account")]
        [RedirectIfNotAuthenticated]
        public async Task<IActionResult> Index(EditAccountDataViewModel model)
        {
            var user = await _unitOfWork.Users.GetUserAsync(User);

            var file = Request.Form.Files.FirstOrDefault();

            if (file != null)
            {
                try
                {
                    var imagePath = await _imageService.UploadAsync(file);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        user.imgPath = imagePath;
                        _unitOfWork.Complete();
                        _toastNotification.AddSuccessToastMessage("Profile picture updated");
                    }
                    else
                    {
                        _toastNotification.AddWarningToastMessage("Error uploading profile picture");
                    }
                }
                catch (Exception ex)
                {
                    _toastNotification.AddWarningToastMessage($"Error: {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }



        [Route("Register")]
        [RedirectIfAuthenticated]
        public ActionResult Register()
            => View(new RegisterViewModel());

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _unitOfWork.Users.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var state = await _accountServies.ConfirmationMail(model.Email);
                if (state)
                {
                    _toastNotification.AddSuccessToastMessage("Plz check your Email :3");
                    return RedirectToAction(nameof(Login));
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(model);
        }


        [Route("Login")]
        [RedirectIfAuthenticated]
        public ActionResult Login()
            => View(new LoginViewModel() { ReturnUrl = HttpContext.Request.Query["returnUrl"] });

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _accountServies.UserByEmailOrName(model.Email);
            bool IsUser = true;

            if (user is null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                IsUser = false;
            }
            if (IsUser && user is not null && !await _unitOfWork.Users.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Invalid login attempt");
                IsUser = false;
            }

            if (IsUser)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result is not null && result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Welcome Back :)");
                    return LocalRedirect(model?.ReturnUrl ?? "~/");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [Route("Logout")]
        [RedirectIfNotAuthenticated]
        public async Task<IActionResult> Logout()
        {
            string returnurl = HttpContext.Request.Query["returnUrl"];
            await _signInManager.SignOutAsync();
            _toastNotification.AddSuccessToastMessage("Back Agian (●'◡'●)");
            if (returnurl is not null && Url.IsLocalUrl(returnurl))
                return Redirect(returnurl);
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ComfirmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            var user = await _unitOfWork.Users.FindByIdAsync(model.userId);
            if (user != null)
            {
                var result = await _unitOfWork.Users.ConfirmEmailAsync(user, model.token);
                if (result.Succeeded)
                    return View();
            }
            return View("Error");
        }

        //[Route("ForgotPassword")]
        //public IActionResult ForgotPassword()
        //    => View(new ForgotPasswordViewModel());

        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Error");

        //    User? user = await _accountServies.UserByEmailOrName(model.item);

        //    if (user is not null)
        //    {
        //        var state = await _accountServies.ResetPasswordToken(model.item);
        //        if (state)
        //        {
        //            _toastNotification.AddSuccessToastMessage("Plz check your Email");
        //            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //        }
        //    }
        //    ModelState.AddModelError("", "Wrong");
        //    return View(model);
        //}
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //    => View();


        //public async Task<IActionResult> ResetPassword(ComfirmViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Error");
        //    var user = await _UnitOfWork.user.FindByIdAsync(model.userId);

        //    if (user != null)
        //        return View(new ResetPasswordViewModel() { Email = user.Email, userId = user.Id, Token = model.token });

        //    return View("Error");
        //}
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Error");

        //    var user = await _UnitOfWork.user.FindByIdAsync(model.userId);
        //    if (user is null || user.Email != model.Email)
        //        return View("Error");

        //    var result = await _UnitOfWork.user.ResetPasswordAsync(user, model.Token, model.Password);
        //    if (result.Succeeded)
        //        return View(nameof(ResetPasswordConfirmation));

        //    return View("Error");
        //}
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //=> View();


        ////Valdition serviers
        //public async Task<JsonResult> IsAlreadySigned(string? Email)
        //{
        //    if (string.IsNullOrEmpty(Email))
        //        return Json(true);

        //    User? user = await _accountServies.UserByEmailOrName(Email);

        //    return Json(user is null);
        //}
        //public async Task<JsonResult> IsUsedName(string? UserName)
        //{
        //    if (string.IsNullOrEmpty(UserName))
        //        return Json(true);

        //    User? user = await _accountServies.UserByEmailOrName(UserName);

        //    return Json(user is null);
        //}
    }
}
