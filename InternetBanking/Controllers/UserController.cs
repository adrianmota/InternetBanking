using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using System.Threading.Tasks;

namespace WebApp.InternetBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

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
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOut();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}