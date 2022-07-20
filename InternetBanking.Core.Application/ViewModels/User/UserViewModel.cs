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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
        public ICollection<PayViewModel> Pays { get; set; }
        public ICollection<TransactionViewModel> Transactions { get; set; }
        public ICollection<ProductViewModel> Beneficiaries { get; set; }
    }
}