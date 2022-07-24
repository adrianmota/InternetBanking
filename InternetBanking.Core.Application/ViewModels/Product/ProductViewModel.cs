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
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public double? Limit { get; set; }
        public string ClientId { get; set; }

        public ICollection<TransactionViewModel> TransactionsIn { get; set; } 
        public ICollection<TransactionViewModel> TransactionsOut { get; set; }
        public ICollection<BeneficiaryViewModel> Beneficiaries { get; set; }
    }
}