using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;
using eShop.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using eShop.Core.Utilities;
using eShop.BLL.Helper;
using System.Linq;
using eShop.BLL.ViewModels;

namespace eShop.MVC.Areas.AuthCustomerAuth.Controllers
{

    [Area("CustomerAuth")]
    [AllowAnonymous]
    public class CustomerAuthController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountServies _accountServies;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly PhotoSettings _photoSettings;
        private readonly IImageServices _imageServices;

        public CustomerAuthController(IImageServices ImageServices, PhotoSettings PhotoSettings, IUnitOfWork UnitOfWork, IMapper mapper, SignInManager<ApplicationUser> signInManager, IAccountServies accountServies, IToastNotification toastNotification)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _accountServies = accountServies;
            _toastNotification = toastNotification;
            _photoSettings = PhotoSettings;
            _imageServices = ImageServices;
        }

        [Authorize]
        [Route("Account")]
        public async Task<IActionResult> Index()
        {
            var user = await _UnitOfWork.Users.GetUserAsync(User);

            if (user is null)
                return RedirectToAction("Index", "Home");

            ViewBag.accept = _photoSettings.AllowedExtensions.Aggregate((x, z) => x + ", " + z);

            var model = _mapper.Map<AccountDataViewModel>(user);

            if (string.IsNullOrEmpty(user.imgPath) != false)
                model.ProfilePicture = await _imageServices.Get(user.imgPath);

            return View(model);
        }

        [HttpPost("Account")]
        public async Task<IActionResult> Index(EditAccountDataViewModel model)
        {
            var user = await _UnitOfWork.Users.GetUserAsync(User);

            var file = Request.Form.Files.FirstOrDefault();

            if (file is not null &&
                file.Length > 0 &&
                _photoSettings.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) &&
                file.Length <= _photoSettings.MaxFileSizeBytes &&
                _photoSettings.AllowedContentTypes.Contains(file.ContentType))
            {
                user.imgPath = await _imageServices.Upload(file);

                _UnitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Profile picture updated");
            }
            else if (file is not null && file.Length > _photoSettings.MaxFileSizeBytes)
                _toastNotification.AddWarningToastMessage($"Error let photo smaller than {_photoSettings.MaxFileSizeBytes / (1024 * 1024)}Mb");
            else if (file is not null && !_photoSettings.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                _toastNotification.AddWarningToastMessage($"Error allowed exteintion is {_photoSettings.AllowedExtensions.Aggregate((x, z) => x + ", " + z)}");

            return RedirectToAction(nameof(Index));
        }



        [Route("Register")]
        public ActionResult Register()
            => View(new RegisterViewModel());

        //[HttpPost("Register")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{

        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var user = _mapper.Map<User>(model);

        //    var result = await _UnitOfWork.user.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        var state = await _accountServies.ConfirmationMail(model.Email);
        //        if (state)
        //        {
        //            _toastNotification.AddSuccessToastMessage("Plz check your Email");
        //            return RedirectToAction(nameof(Login));
        //        }
        //    }
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error.Description);
        //    }

        //    return View(model);
        //}


        //[Route("Login")]
        //public ActionResult Login(string returnUrl = null)
        //    => View(new LoginViewModel() { ReturnUrl = returnUrl });

        //[HttpPost("Login")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //    User? user = await _accountServies.UserByEmailOrName(model.Email);

        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "Email or Password is worng");
        //        return View(model);
        //    }
        //    if (!await _UnitOfWork.user.IsEmailConfirmedAsync(user))
        //    {
        //        ModelState.AddModelError("", "Please confirm your membership with the link sent to your e-mail account.");
        //        return View(model);
        //    }
        //    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
        //    if (result.Succeeded)
        //    {
        //        _toastNotification.AddSuccessToastMessage("Welcame Back");
        //        return LocalRedirect(model?.ReturnUrl ?? "~/");
        //    }

        //    ModelState.AddModelError("", "The username or password entered is incorrect");
        //    return View(model);
        //}

        //[Route("Logout")]
        //public async Task<IActionResult> Logout(string returnUrl = null)
        //{
        //    await _signInManager.SignOutAsync();
        //    _toastNotification.AddSuccessToastMessage("Back Agian (●'◡'●)");
        //    if (returnUrl is not null && Url.IsLocalUrl(returnUrl))
        //        return LocalRedirect(returnUrl);
        //    else
        //        return RedirectToAction("Index", "Home", new { area = "" });
        //}
        //[Route("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail(ComfirmViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Error");
        //    }
        //    var user = await _UnitOfWork.user.FindByIdAsync(model.userId);
        //    if (user != null)
        //    {
        //        var result = await _UnitOfWork.user.ConfirmEmailAsync(user, model.token);
        //        if (result.Succeeded)
        //            return View();
        //    }
        //    return View("Error");
        //}

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
