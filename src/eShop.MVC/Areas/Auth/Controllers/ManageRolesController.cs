using AutoMapper;
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Areas.Auth.Controllers
{
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [Area("Auth")]
    public class ManageRolesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageRolesController(IUnitOfWork UnitOfWork, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? role)
        {
            List<AccountViewModel> model;
            if (string.IsNullOrEmpty(role))
                model = _mapper.Map<List<AccountViewModel>>(await _unitOfWork.Users.GetAllAsync());
            else
            {
                model = _mapper.Map<List<AccountViewModel>>(await _unitOfWork.Users.GetAllByRoleAsync(role));
            }
            return View(model ?? default);
        }
        [HttpPost]
        public IActionResult AddRole()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddRoleTemporarily()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RevokeRole()
        {
            return RedirectToAction("Index");
        }
    }
}
