using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Admin;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Dtos.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.InternetBanking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public AdminController(ITransactionService transactionService, IUserService userService, IProductService productService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<TransactionViewModel> all = await _transactionService.GetAllViewModel();

            List<TransactionViewModel> transactions = all.FindAll(t => 
                                        t.Type == (int)TransactionType.Transaction || 
                                        t.Type == (int)TransactionType.CashAdvance);

            List<TransactionViewModel> pays = all.FindAll(t =>
                                        t.Type == (int)TransactionType.Direct ||
                                        t.Type == (int)TransactionType.Loan ||
                                        t.Type == (int)TransactionType.CreditCard ||
                                        t.Type == (int)TransactionType.ForBeneficiary);

            List<TransactionViewModel> today = await _transactionService.GetTodayTransactions();

            List<TransactionViewModel> todayTransactions = today.FindAll(t =>
                                        t.Type == (int)TransactionType.Transaction ||
                                        t.Type == (int)TransactionType.CashAdvance);

            List<TransactionViewModel> todayPays = today.FindAll(t =>
                                        t.Type == (int)TransactionType.Direct ||
                                        t.Type == (int)TransactionType.Loan ||
                                        t.Type == (int)TransactionType.CreditCard ||
                                        t.Type == (int)TransactionType.ForBeneficiary);

            List<UserViewModel> users = await _userService.GetAllViewModel();
            var products = await _productService.GetAllViewModel();

            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                TodayTransactions = todayTransactions.Count,
                TodayTransactionsAmount = todayTransactions.Sum(t => t.Amount),
                TodayPays = todayPays.Count,
                TodayPaysAmount = todayPays.Sum(t => t.Amount),

                TotalTransactions = transactions.Count,
                TotalTransactionsAmount = transactions.Sum(t => t.Amount),
                TotalPays = pays.Count,
                TotalPaysAmount = pays.Sum(p => p.Amount),

                ActiveUsers = users.FindAll(u => u.IsActive && u.Role == Roles.Client.ToString()).Count,
                InactiveUsers = users.FindAll(u => !u.IsActive).Count,
                ProductsCount = products.Count
            };

            return View(dashboardViewModel);
        }
    }
}