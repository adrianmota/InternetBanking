using InternetBanking.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public string ClientId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el numero de cuenta")]
        public int AccountId { get; set; }
    }
}
