// Controllers/AdminDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Controllers;

using BLL.Services.Abstraction;
using Core.Utilities;

[Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
[Route("/admin/dashboard")]
public class AdminDashboardController(IDashboardServices dashboardServices) : Controller {

    public async Task<IActionResult> Index() {
        var viewModel = await dashboardServices.RetrieveDashboardDataAsync();
        return View(viewModel);
    }
}