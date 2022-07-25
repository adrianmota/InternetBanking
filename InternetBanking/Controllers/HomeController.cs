using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AuthenticationResponse _user;

        public HomeController(IProductService productService, IMapper mapper,IHttpContextAccessor httpContext)
        {
            _productService = productService;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index()
        {
            ProductListViewModel productList = new();

            List<ProductViewModel> products = await _productService.GetProductsByUserId(_user.Id);

            productList.Accounts = new();
            productList.CreditCards = new();
            productList.Loans = new();

            foreach (ProductViewModel product in products)
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

        public IActionResult CreditCardPay()
        {
            return View();
        }

        public IActionResult ForBeneficiaryPay()
        {
            return View();
        }

        public IActionResult LoanPay()
        {
            return View();
        }

        public IActionResult ExpressPay()
        {
            return View();
        }

        public IActionResult Beneficiaries()
        {
            return View();
        }

        public IActionResult MoneyAdvance()
        {
            return View();
        }

        public IActionResult Transaction()
        {
            return View();
        }
    }
}