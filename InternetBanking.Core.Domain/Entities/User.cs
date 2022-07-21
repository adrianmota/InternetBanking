using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //For difference between Admin and Client
        public int Type { get; set; } 
        public bool Active { get; set; }

        #region relations
        //Navigation props
        public ICollection<Product> Products { get; set; } //List of Accounts and Products of the Client
        public ICollection<Pay> Pays { get; set; } //List of Pays that have been made by the Client
        public ICollection<Transaction> Transactions { get; set; } //List of Transactions that have been made by the Client
        public ICollection<Product> Beneficiaries { get; set; } //List of Accounts that are Beneficiaries of this Client
        public List<Beneficiary> ClientBeneficiaries { get; set; } //M-M Relation with Products for the Benefiriaries
        #endregion
    }
}