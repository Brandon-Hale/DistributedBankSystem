using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!Request.Cookies.ContainsKey("Authenticated") && !(Request.Cookies["Authenticated"] == "true"))
            {
                return View("Login");
            }
            if (Request.Cookies["UserType"] == "admin")
            {
                ViewBag.User = "admin";
            }
            return View();
        }

        public IActionResult Transaction()
        {
            if (!Request.Cookies.ContainsKey("Authenticated") && !(Request.Cookies["Authenticated"] == "true"))
            {
                return View("Login");
            }
            if (Request.Cookies["UserType"] == "admin")
            {
                ViewBag.User = "admin";
            }
            return View();
        }

        public IActionResult Login()
        {
            if (!Request.Cookies.ContainsKey("Authenticated") && !(Request.Cookies["Authenticated"] == "true"))
            {
                return View("Login");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if(Request.Cookies.ContainsKey("Authenticated"))
            {
                Response.Cookies.Delete("Authenticated");

            }
            if (Request.Cookies.ContainsKey("User"))
            {
                Response.Cookies.Delete("User");

            }
            if (Request.Cookies.ContainsKey("UserType"))
            {
                Response.Cookies.Delete("UserType");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}