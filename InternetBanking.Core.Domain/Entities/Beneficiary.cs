using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiary : AuditableBaseEntity
    {
        //Entity for the M-M Relation between Users and Products for the Beneficiaries
        public string ClientId { get; set; } //The Client who have the Beneficiary
        public int AccountId { get; set; } //The Account which is Beneficiary of the Client

        //public User Client { get; set; }
        public Product Account { get; set; }
    }
}