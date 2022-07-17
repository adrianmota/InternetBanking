using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        //All types of products: SavingAccounts, CreditCrads and Loans
        public int Type { get; set; }
        public double Amount { get; set; }

        //Only for CreditCards
        public double? Limit { get; set; }

        #region relations
        public int ClientId { get; set; }

        //Navigation props
        public User Client { get; set; }
        public ICollection<Pay> PaysIn { get; set; } //Pays that enters the Account
        public ICollection<Pay> PaysOut { get; set; } //Pays that come out the Account
        public ICollection<Transaction> TransactionsIn { get; set; } //Transactions that enters the Account
        public ICollection<Transaction> TransactionsOut { get; set; } //Transactions that come out the Account
        public ICollection<User> Beneficiaries { get; set; } //List of Clients that have this Account as Benericiary
        public List<Beneficiary> ClientBeneficiaries { get; set; } //M-M Relation with Users for the Benefiriaries
        #endregion
    }
}