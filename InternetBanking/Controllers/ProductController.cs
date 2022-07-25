using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Dtos.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.InternetBanking.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IUserService userService, IMapper mapper)
        {
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string userId)
        {
            ProductListViewModel productList = new();
            SaveUserViewModel saveUser = await _userService.GetByIdSaveViewModel(userId);
            productList.User = _mapper.Map<UserViewModel>(saveUser);

            List<ProductViewModel> products = await _productService.GetProductsByUserId(userId);

            productList.Accounts = new();
            productList.CreditCards = new();
            productList.Loans = new();

            foreach(ProductViewModel product in products)
            {
                if (product.Type == (int)ProductType.SavingAccount || product.Type == (int)ProductType.MainSavingAccount)
                {
                    productList.Accounts.Add(product);
                }
                else if (product.Type == (int)ProductType.CreditCard)
                {
                    productList.CreditCards.Add(product);
                }
                else if (product.Type == (int)ProductType.Loan)
                {
                    productList.Loans.Add(product);
                }
            }

            return View(productList);
        }

        public IActionResult Add(string userId)
        {
            return View("SaveProduct",new SaveProductViewModel()
            {
                ClientId = userId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(SaveProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var product = await _productService.Add(vm);

            if (product == null)
            {
                ModelState.AddModelError("insertError", "Error al tratar de guardar el producto");
                return View(vm);
            }

            if (vm.Type == (int)ProductType.Loan)
            {
                await _productService.AddAmountToMainAccount(vm.ClientId, vm.Amount);
            }

            return RedirectToAction("Index", new { userId = vm.ClientId });            
        }

        //public async Task<IActionResult> Edit(int productId)
        //{
        //    return View("SaveProduct", await _productService.GetByIdSaveViewModel(productId));
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(SaveProductViewModel vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }

        //    await _productService.Update(vm, vm.Id);

        //    return RedirectToAction("Index", new { userId = vm.ClientId });
        //}

        public async Task<IActionResult> Delete(int productId, string clientId)
        {
            SaveProductViewModel product = await _productService.GetByIdSaveViewModel(productId);

            switch (product.Type)
            {
                case (int)ProductType.MainSavingAccount:
                    return RedirectToAction("Index", new { userId=clientId });

                 case (int)ProductType.Loan:
                 case (int)ProductType.CreditCard:
                    if (product.Amount > 0)
                        ViewBag.Blocked = "1";
                    break;           
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveProductViewModel saveProduct)
        {
            await _productService.Delete(saveProduct.Id);

            if (saveProduct.Type == (int)ProductType.SavingAccount)
                await _productService.AddAmountToMainAccount(saveProduct.ClientId, saveProduct.Amount);

            return RedirectToAction("Index", new { userId = saveProduct.ClientId });
        }
    }
}
