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

        public IActionResult Create()
        {
            ViewBag.Roles = _userService.GetAllRoles();
            return View("SaveUser", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel saveViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _userService.GetAllRoles();
                return View("SaveUser", saveViewModel);
            }

            RegisterResponse response = await _userService.Add(saveViewModel);

            if (response != null && response.HasError)
            {
                saveViewModel.HasError = response.HasError;
                saveViewModel.Error = response.Error;
                return View("SaveUser", saveViewModel);
            }

            return RedirectToRoute(new { controller = "User", action = "AdministrateUsers" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            SaveUserViewModel saveViewModel = await _userService.GetByIdSaveViewModel(id);
            ViewBag.Roles = _userService.GetAllRoles();
            return View("SaveUser", saveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel saveViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _userService.GetAllRoles();
                return View("SaveUser", saveViewModel);
            }

            return RedirectToRoute(new { controller = "User", action = "AdministrateUsers" });
        }

        public async Task<IActionResult> AdministrateUsers()
        {
            return View(await _userService.GetAllViewModel());
        }

        public async Task<IActionResult> SetUserStatus(string id)
        {
            await _userService.SetUserStatus(id);
            return RedirectToRoute(new { controller = "User", action = "AdministrateUsers" });
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOut();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}