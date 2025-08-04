using AutoMapper;
using eShop.BLL.Services.Abstraction;
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;
using eShop.Core.Utilities;
using eShop.DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Areas.Auth.Controllers
{
    [Authorize(Roles = $"{Roles.SuperAdmin}")]
    [Area("Auth")]
    public class ManageRolesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBackgroundJobServices _backgroundJobService;
        private readonly IImageServices _imageServices;

        public ManageRolesController(
            IImageServices imageServices,
            IBackgroundJobServices BackgroundJobService,
            IUnitOfWork UnitOfWork,
            IMapper mapper,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _backgroundJobService = BackgroundJobService;
            _imageServices = imageServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? role)
        {
            List<AccountViewModel> model = [];
            IList<ApplicationUser> users = [];
            if (string.IsNullOrEmpty(role))
                users = await _unitOfWork.Users.GetAllAsync();
            else
            {
                users = await _unitOfWork.Users.GetAllByRoleAsync(role);
            }

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                model.Add(_mapper.Map<AccountViewModel>(user));
                if (user?.imgPath is not null)
                    model[i].ProfilePicture = await _imageServices.GetAsync(user.imgPath);
            }

            return View(model);
        }

        [HttpGet("/api/search-users-by-roles")]
        public async Task<IActionResult> SearchUsersByRoles(Guid role) {
            List<AccountViewModel> users;
            
            if(role != Guid.Empty)
                users= _mapper.Map<List<AccountViewModel>>(await _unitOfWork.Users.GetAllByRoleAsync(role));
            else
                users= _mapper.Map<List<AccountViewModel>>(await _unitOfWork.Users.GetAllAsync());
            
            return Ok(users ?? []);
        }

        [HttpGet("/api/addrole")]
        public async Task<IActionResult> AddRole(Guid Id, Guid role)
        {
            if (Id == Guid.Empty || role == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _unitOfWork.Users.AddToRoleAsync(Id, role);
            if (result.Succeeded)
            {
                var user = await _unitOfWork.Users.FindByIdAsync(Id.ToString());
                if (user != null)
                {
                    await _signInManager.RefreshSignInAsync(user);
                }
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", string.Join(", ", result.Errors));
                return NotFound(ModelState);
            }
        }

        [HttpGet("/api/add-role-temporarily")]
        public async Task<IActionResult> AddRoleTemporarily(Guid Id, Guid role, DateTime time)
        {
            if (Id == Guid.Empty || role == Guid.Empty)
            {
                return BadRequest("Invalid data.");
            }

            TimeSpan delay = time - DateTime.UtcNow;
            delay = delay < TimeSpan.Zero ? TimeSpan.Zero : delay;

            if (time == default || time < new DateTime(2025, 1, 1) || time > new DateTime(2100, 1, 1))
            {
                return BadRequest("Invalid time.");
            }

            var result = await _unitOfWork.Users.AddToRoleAsync(Id, role);
            if (result.Succeeded)
            {
                if (await _unitOfWork.Users.FindByIdAsync(Id.ToString()) is Core.Entities.ApplicationUser user)
                {
                    await _signInManager.RefreshSignInAsync(user);
                }
            }
            else
            {
                ModelState.AddModelError("", string.Join(", ", result.Errors));
                return NotFound(ModelState);
            }
            _backgroundJobService.Schedule(
                () => _unitOfWork.Users.RemoveFromRoleAsync(Id, role)
                , delay);

            return NoContent();
        }

        [HttpGet("/api/roles/{Id}")]
        public async Task<IActionResult> GetUserRoles(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }

            var roles = await _unitOfWork.Users.GetRolesAsync(Id);
            return Ok(roles.Select(r => new
            {
                r.Id,
                r.Name,
            }));
        }

        [HttpGet("/api/roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return Ok(roles.Select(r => new
            {
                r.Id,
                r.Name,
            }));
        }

        [HttpDelete("/api/revoke-role")]
        public async Task<IActionResult> RevokeRole(Guid id, Guid role)
        {
            await _unitOfWork.Users.RemoveFromRoleAsync(id, role);
            return NoContent();
        }
    }
}
