﻿using InternetBanking.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public string ClientId { get; set; }
        public int AccountId { get; set; }
        public ProductViewModel Account { get; set; }
    }
}
