using InternetBanking.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transaction
{
    public class MoneyAdvanceViewModel
    {
        public List<ProductViewModel> CreditCards { get; set; }
        public List<ProductViewModel> Accounts { get; set; }
        public SaveTransactionViewModel Transaction { get; set; }
    }
}
