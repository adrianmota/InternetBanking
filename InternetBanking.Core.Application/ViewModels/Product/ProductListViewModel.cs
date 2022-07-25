using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Transaction;
using InternetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class ProductListViewModel
    {
        public UserViewModel User { get; set; }
        public List<ProductViewModel> Accounts { get; set; }
        public List<ProductViewModel> CreditCards { get; set; }
        public List<ProductViewModel> Loans { get; set; }
    }
}