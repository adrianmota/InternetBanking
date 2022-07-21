using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
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