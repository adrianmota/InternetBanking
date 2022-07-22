﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Enums
{
    public enum TransactionType
    {
        Transaction=1,
        CashAdvance,
        Direct,
        CreditCard,
        Loan,
        ForBeneficiary
    }
}
