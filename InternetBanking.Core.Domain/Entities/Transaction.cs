using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Transaction : AuditableBaseEntity
    {
        public int Type { get; set; } //Because CashAdvances are also storaged here; Types: Transaction and CashAdvance 
        public double Amount { get; set; }

        #region relations
        public int ClientId { get; set; }
        public int AccountFromId { get; set; } //The Account from the Transaction is made
        public int AccountToId { get; set; } //The Account which is going to receive the Transaction

        //Navigation props
        public User Client { get; set; }
        public Product AccountFrom { get; set; }
        public Product AccountTo { get; set; }
        #endregion
    }
}
