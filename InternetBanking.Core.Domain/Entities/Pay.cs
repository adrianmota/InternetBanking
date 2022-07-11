using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Pay : AuditableBaseEntity
    {
        public int Type { get; set; } //For all types of Pays: Direct (see explanation below), CreditCard, Loan and ForBeneficiary
        public double Amount { get; set; }

        #region relations
        public int ClientId { get; set; }
        public int AccountFromId { get; set; } //The Account from the Pay is made
        public int AccountToId { get; set; } //The Account which is going to receive the Pay

        //Navigation props
        public User Client { get; set; }
        public Product AccountFrom { get; set; }
        public Product AccountTo { get; set; }
        #endregion

        //Explanation for Direct: Le puse Direct y no Express porque Express en ingles es como "Rapido" xd, y un pago
        //                      expreso es como un pago directo entre usuario y usuario, no necesariamente rapido
    }
}
