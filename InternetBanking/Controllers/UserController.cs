using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.Enums;
using WebApp.InternetBanking.Middlewares;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.InternetBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            AuthenticationResponse response = await _userService.LoginAsync(login);

            if (response != null && response.HasError)
            {
                login.HasError = true;
                login.Error = response.Error;
                return View(login);
            }

            HttpContext.Session.Set<AuthenticationResponse>("user", response);

            bool isAdmin = response != null ? response.Roles.Any(role => role == "Admin") : false;
            bool isClient = response != null ? response.Roles.Any(role => role == "Client") : false;
            string controller = "";

            if (isAdmin)
                controller = "Admin";
            else if (isClient)
                controller = "Home";

            return RedirectToRoute(new { controller = controller, action = "Index" });
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOut();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}