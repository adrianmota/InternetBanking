using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.InternetBanking.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string userId)
        {
            SaveUserViewModel saveUser = await _userService.GetByIdSaveViewModel(userId);
            UserViewModel uservm = new()
            {
                Id=saveUser.Id,
                FirstName=saveUser.FirstName,
                LastName=saveUser.LastName,
                Email=saveUser.Email,
                DNI=saveUser.DNI,
                UserName=saveUser.UserName
            };

            uservm.Products = await _productService.GetProductsByUserId(userId);
            return View(uservm);
        }

        public IActionResult Add(string userId)
        {
            return View("SaveProduct",new SaveProductViewModel()
            {
                ClientId = userId
            });
        }

    }
}
