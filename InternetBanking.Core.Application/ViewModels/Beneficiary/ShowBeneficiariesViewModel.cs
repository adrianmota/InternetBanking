using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary
{
    public class ShowBeneficiariesViewModel
    {
        public List<BeneficiaryViewModel> Beneficiaries { get; set; }
        public SaveBeneficiaryViewModel SaveBeneficiary { get; set; }
    }
}
