﻿using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    [Authorize(Roles ="Client")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AuthenticationResponse _user;

        private readonly ShowBeneficiariesViewModel benlist = new();

        public HomeController(IProductService productService, IMapper mapper, IHttpContextAccessor httpContext,
            ITransactionService transactionService, IBeneficiaryService beneficiaryService, IUserService userService)
        {
            _productService = productService;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
            _transactionService = transactionService;
            _beneficiaryService = beneficiaryService;
            _userService = userService;
        }

 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> CreditCardPay()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            CreditCardPayViewModel cardViewModel = new();

            cardViewModel.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            cardViewModel.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

            if (cardViewModel.CreditCards.Count == 0)
            {
                ModelState.AddModelError("userWithoutCards", "No tienes ninguna tarjeta de credito asignada para pagar");
                return View(new CreditCardPayViewModel());
            }

            cardViewModel.Transaction = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.CreditCard
            };

            return View(cardViewModel);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> CreditCardPay(CreditCardPayViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            if (vm.Transaction.Amount < 0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Transaction.AccountFromId, vm.Transaction.Amount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa el limite de la cuenta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            bool isMoreThanDebt= !(await _productService.CheckAccountAmount(vm.Transaction.AccountToId, vm.Transaction.Amount));
            double trueAmount;
            var card = await _productService.GetByIdSaveViewModel(vm.Transaction.AccountToId);

            if (isMoreThanDebt)
            {
                trueAmount = card.Amount;
            }
            else
            {
                trueAmount = vm.Transaction.Amount;
            }

            vm.Transaction.Amount = trueAmount;

            var transaction = await _transactionService.Add(vm.Transaction);

            if (transaction == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error pagando a la tarjeta de credito");
                return View(vm);
            }

            await _productService.SubstractAmountToProduct(transaction.AccountToId, transaction.Amount);

            await _productService.SubstractAmountToProduct(transaction.AccountFromId, transaction.Amount);

            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> ForBeneficiaryPay()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            BeneficiariesPayViewModel benVM = new();

            benVM.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            var beneficiaries = await _beneficiaryService.GetBeneficiariesByUser(_user.Id);
            List<BeneficiaryViewModel> beneficiaryList = new(beneficiaries);

            for (int i = 0; i < beneficiaries.Count; i++)
            {
                var beneficiary = beneficiaries[i];
                var account = await _productService.GetByIdSaveViewModel(beneficiary.AccountId);
                var user = await _userService.GetByIdSaveViewModel(account.ClientId);

                beneficiaryList[i].Name = user.FirstName;
                beneficiaryList[i].LastName = user.LastName;
            }

            if (beneficiaryList.Count == 0)
            {
                ModelState.AddModelError("userWithoutCards", "No tienes ninguna cuenta agregada como beneficiaria");
                return View(new BeneficiariesPayViewModel());
            }

            benVM.Beneficiaries = beneficiaryList;

            benVM.Pay = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.ForBeneficiary
            };

            return View(benVM);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> ForBeneficiaryPay(BeneficiariesPayViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                var beneficiaries = await _beneficiaryService.GetBeneficiariesByUser(_user.Id);
                List<BeneficiaryViewModel> beneficiaryList = new(beneficiaries);

                for (int i = 0; i < beneficiaries.Count; i++)
                {
                    var beneficiary = beneficiaries[i];
                    var account = await _productService.GetByIdSaveViewModel(beneficiary.AccountId);
                    var user = await _userService.GetByIdSaveViewModel(account.ClientId);

                    beneficiaryList[i].Name = user.FirstName;
                    beneficiaryList[i].LastName = user.LastName;
                }

                if (beneficiaryList.Count == 0)
                {
                    ModelState.AddModelError("userWithoutCards", "No tienes ninguna cuenta agregada como beneficiaria");
                    return View(new BeneficiariesPayViewModel());
                }

                vm.Beneficiaries = beneficiaryList;

                return View(vm);
            }

            if (vm.Pay.Amount < 0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                var beneficiaries = await _beneficiaryService.GetBeneficiariesByUser(_user.Id);
                List<BeneficiaryViewModel> beneficiaryList = new(beneficiaries);

                for (int i = 0; i < beneficiaries.Count; i++)
                {
                    var beneficiary = beneficiaries[i];
                    var account = await _productService.GetByIdSaveViewModel(beneficiary.AccountId);
                    var user = await _userService.GetByIdSaveViewModel(account.ClientId);

                    beneficiaryList[i].Name = user.FirstName;
                    beneficiaryList[i].LastName = user.LastName;
                }

                if (beneficiaryList.Count == 0)
                {
                    ModelState.AddModelError("userWithoutCards", "No tienes ninguna cuenta agregada como beneficiaria");
                    return View(new BeneficiariesPayViewModel());
                }

                vm.Beneficiaries = beneficiaryList;

                return View(vm);
            }

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Pay.AccountFromId, vm.Pay.Amount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa el limite de la cuenta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                var beneficiaries = await _beneficiaryService.GetBeneficiariesByUser(_user.Id);
                List<BeneficiaryViewModel> beneficiaryList = new(beneficiaries);

                for (int i = 0; i < beneficiaries.Count; i++)
                {
                    var beneficiary = beneficiaries[i];
                    var account = await _productService.GetByIdSaveViewModel(beneficiary.AccountId);
                    var user = await _userService.GetByIdSaveViewModel(account.ClientId);

                    beneficiaryList[i].Name = user.FirstName;
                    beneficiaryList[i].LastName = user.LastName;
                }

                if (beneficiaryList.Count == 0)
                {
                    ModelState.AddModelError("userWithoutCards", "No tienes ninguna cuenta agregada como beneficiaria");
                    return View(new BeneficiariesPayViewModel());
                }

                vm.Beneficiaries = beneficiaryList;

                return View(vm);
            }

            var pay = await _transactionService.Add(vm.Pay);

            if (pay == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error al pagar a la cuenta solicitada");
                return View(vm);
            }

            await _productService.SubstractAmountToProduct(pay.AccountFromId, pay.Amount);
            await _productService.AddAmountToProduct(pay.AccountToId, pay.Amount);

            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> LoanPay()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            LoanPayViewModel loanViewModel = new();

            loanViewModel.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            loanViewModel.Loans = products.FindAll(p => p.Type == (int)ProductType.Loan);

            if (loanViewModel.Loans.Count == 0)
            {
                ModelState.AddModelError("userWithoutCards", "No tienes ningun prestamos que pagar");
                return View(new LoanPayViewModel());
            }

            loanViewModel.Transaction = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.CreditCard
            };

            return View(loanViewModel);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> LoanPay(LoanPayViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.Loans = products.FindAll(p => p.Type == (int)ProductType.Loan);

                return View(vm);
            }

            if (vm.Transaction.Amount < 0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.Loans = products.FindAll(p => p.Type == (int)ProductType.Loan);

                return View(vm);
            }

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Transaction.AccountFromId, vm.Transaction.Amount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa el limite de la cuenta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.Loans = products.FindAll(p => p.Type == (int)ProductType.Loan);

                return View(vm);
            }

            bool isMoreThanDebt = !(await _productService.CheckAccountAmount(vm.Transaction.AccountToId, vm.Transaction.Amount));
            var card = await _productService.GetByIdSaveViewModel(vm.Transaction.AccountToId);

            double trueAmount;
            if (isMoreThanDebt)
            {
                trueAmount = card.Amount;
            }
            else
            {
                trueAmount = vm.Transaction.Amount;
            }

            vm.Transaction.Amount = trueAmount;

            var transaction = await _transactionService.Add(vm.Transaction);

            if (transaction == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error pagando el prestamo");
                return View(vm);
            }

            await _productService.SubstractAmountToProduct(transaction.AccountToId, transaction.Amount);

            await _productService.SubstractAmountToProduct(transaction.AccountFromId, transaction.Amount);

            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> ExpressPay()
        {
            var products = await _productService.GetProductsByUserId(_user.Id);
            ExpressPayViewModel expPay = new();

            expPay.Accounts = products.FindAll(p =>
                            p.Type == (int)ProductType.SavingAccount ||
                            p.Type == (int)ProductType.MainSavingAccount);

            expPay.Pay = new()
            {
                ClientId = _user.Id,
                Type = (int)TransactionType.Direct
            };

            return View(expPay);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> ExpressPay(ExpressPayViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            if (vm.Pay.Amount < 0)
            {
                ModelState.AddModelError("amountNegative", "El monto no puede ser negativo");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            var account = await _productService.GetByIdSaveViewModel(vm.Pay.AccountToId);
            if (account == null)
            {
                ModelState.AddModelError("accountNumberNotValid", "El numero de cuenta ingresado no es valido");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Pay.AccountFromId, vm.Pay.Amount);
            if (!isEnoughAmount)
            {
                ModelState.AddModelError("notEnoughAmount", "La transferencia sobrepasa el limite de la cuenta");

                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            var pay = await _transactionService.Add(vm.Pay);

            if (pay == null)
            {
                ModelState.AddModelError("errorTransferingAmount", "Error al pagar a la cuenta soicitada");
                return View(vm);
            }

            await _productService.SubstractAmountToProduct(pay.AccountFromId, pay.Amount);
            await _productService.AddAmountToProduct(pay.AccountToId, pay.Amount);

            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> Beneficiaries(int errorType = 0)
        {
            var beneficiaries = await _beneficiaryService.GetBeneficiariesByUser(_user.Id);
            List<BeneficiaryViewModel> beneficiaryList = new(beneficiaries);

            for (int i = 0; i < beneficiaries.Count; i++)
            {
                var beneficiary = beneficiaries[i];
                var account = await _productService.GetByIdSaveViewModel(beneficiary.AccountId);
                var user = await _userService.GetByIdSaveViewModel(account.ClientId);

                beneficiaryList[i].Name = user.FirstName;
                beneficiaryList[i].LastName = user.LastName;
            }

            ShowBeneficiariesViewModel showBeneficiaries = new()
            {
                Beneficiaries = beneficiaryList,
                SaveBeneficiary = new()
                {
                    ClientId = _user.Id
                }
            };

            switch (errorType)
            {
                case 1:
                    ModelState.AddModelError("modelNotValid", "Debe proporcionar un numero de cuenta");
                    break;

                case 2:
                    ModelState.AddModelError("accountNotExists", "Este numero de cuenta no existe");
                    break;

                case 3:
                    ModelState.AddModelError("accountNotValid", "El numero de cuenta proporcionado no corresponde a una cuenta de ahorros");
                    break;

                case 4:
                    ModelState.AddModelError("yourAccount", "No puedes seleccionar una de tus cuentas como beneficiaria");
                    break;

                case 5:
                    ModelState.AddModelError("errorSaving", "Error al tratar de guardar el beneficiario");
                    break;


            }
            return View(showBeneficiaries);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> AddBeneficiary(ShowBeneficiariesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Beneficiaries", new { errorType = 1 });
            }

            var account = await _productService.GetByIdSaveViewModel(vm.SaveBeneficiary.AccountId);
            if (account == null)
            {
                return RedirectToAction("Beneficiaries", new { errorType = 2 });
            }

            if (account.Type != (int)ProductType.SavingAccount && account.Type != (int)ProductType.MainSavingAccount)
            {
                return RedirectToAction("Beneficiaries", new { errorType = 3 });
            }

            if (account.ClientId == _user.Id)
            {
                return RedirectToAction("Beneficiaries", new { errorType = 4 });
            }

            var beneficiary = await _beneficiaryService.Add(vm.SaveBeneficiary);
            if (beneficiary == null)
            {
                return RedirectToAction("Beneficiaries", new { errorType = 5 });
            }

            return RedirectToAction("Beneficiaries");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> DeleteBeneficiary(int accountId)
        {
            await _beneficiaryService.DeleteByUserAndAccount(_user.Id, accountId);
            return RedirectToAction("Beneficiaries");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> MoneyAdvance(MoneyAdvanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                vm.CreditCards = products.FindAll(p => p.Type == (int)ProductType.CreditCard);

                return View(vm);
            }

            if (vm.Transaction.Amount < 0)
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<IActionResult> Transaction(TransferMoneyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserId(_user.Id);

                vm.Accounts = products.FindAll(p =>
                                p.Type == (int)ProductType.SavingAccount ||
                                p.Type == (int)ProductType.MainSavingAccount);

                return View(vm);
            }

            if (vm.Transaction.AccountFromId == vm.Transaction.AccountToId)
            {
                ModelState.AddModelError("sameAccount", "No puedes seleccionar la misma cuenta inicial y receptora");

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

            bool isEnoughAmount = await _productService.CheckAccountAmount(vm.Transaction.AccountFromId, vm.Transaction.Amount);
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