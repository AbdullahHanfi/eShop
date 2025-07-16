using eShop.DAL.Interface;
using eShop.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About() => View();

        public IActionResult Privacy() => View();

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                500 => View("ServerError"),
                404 => View("NotFound"),
                403 => View("AccessDenied"),
                401 => View("Unauthorized"),
                _ => View("Error"),
            };
        } 
    }
}