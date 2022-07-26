using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.Transaction;
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
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AuthenticationResponse _user;

        public HomeController(IProductService productService, IMapper mapper,IHttpContextAccessor httpContext, ITransactionService transactionService)
        {
            _productService = productService;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
            _transactionService = transactionService;
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

        public async Task<IActionResult> MoneyAdvance()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            MoneyAdvanceViewModel moneyAdvance = new();

            moneyAdvance.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            moneyAdvance.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

            if (moneyAdvance.CreditCards.Count == 0)
            {
                ModelState.AddModelError("userWithoutCards", "No tienes ninguna tarjeta de credito asignada para realizar esta transaccion");
                return View(new MoneyAdvanceViewModel());
            }

            moneyAdvance.Transaction = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.CashAdvance
            };

            return View(moneyAdvance);
        }

        [HttpPost]
        public async Task<IActionResult> MoneyAdvance(MoneyAdvanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Transaction.Amount<0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            var creditCard = await _productService.GetByIdSaveViewModel(vm.Transaction.AccountFromId);
            double totalAmount = creditCard.Amount + vm.Transaction.Amount;

            bool isEnoughAmount = await _productService.CheckCreditCardLimit(vm.Transaction.AccountFromId, totalAmount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa el limite de la tarjeta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            var transaction = await _transactionService.Add(vm.Transaction);

            if (transaction == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error transfiriendo el dinero");
                return View(vm);
            }

            await _productService.AddAmountToProduct(transaction.AccountToId, transaction.Amount);

            double cardAmount = Math.Round(transaction.Amount * 1.0625, 2);
            await _productService.AddAmountToProduct(transaction.AccountFromId, cardAmount);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Transaction()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            TransferMoneyViewModel transferVM = new();

            transferVM.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            if (transferVM.Accounts.Count == 1)
            {
                ModelState.AddModelError("onlyOneAccount", "No puedes hacer una transferencia debido a que solo tienes la cuenta de ahorros principal");
                return View(new TransferMoneyViewModel());
            }

            transferVM.Transaction = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.CashAdvance
            };

            return View(transferVM);
        }

        [HttpPost]
        public async Task<IActionResult> Transaction(TransferMoneyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Transaction.AccountFromId == vm.Transaction.AccountToId)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            if (vm.Transaction.Amount < 0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            var firstAccount = await _productService.GetByIdSaveViewModel(vm.Transaction.AccountFromId);
            double totalAmount = firstAccount.Amount + vm.Transaction.Amount;

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Transaction.AccountFromId, totalAmount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa la cantidad de dinero en la primera cuenta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            var transaction = await _transactionService.Add(vm.Transaction);

            if (transaction == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error transfiriendo el dinero");
                return View(vm);
            }

            await _productService.SubstractAmountToProduct(transaction.AccountFromId, transaction.Amount);
            await _productService.AddAmountToProduct(transaction.AccountToId, transaction.Amount);

            return RedirectToAction("Index");
        }
    }
}