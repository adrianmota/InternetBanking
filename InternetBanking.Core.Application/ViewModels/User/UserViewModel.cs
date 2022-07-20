using InternetBanking.Core.Application.ViewModels.Pay;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }
        public bool Active { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
        public ICollection<PayViewModel> Pays { get; set; }
        public ICollection<TransactionViewModel> Transactions { get; set; }
        public ICollection<ProductViewModel> Beneficiaries { get; set; }
    }
}