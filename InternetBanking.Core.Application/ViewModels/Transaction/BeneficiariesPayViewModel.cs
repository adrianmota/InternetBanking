using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transaction
{
    public class BeneficiariesPayViewModel
    {
        public List<BeneficiaryViewModel> Beneficiaries { get; set; }
        public List<ProductViewModel> Accounts { get; set; }
        public SaveTransactionViewModel Pay { get; set; }
    }
}
