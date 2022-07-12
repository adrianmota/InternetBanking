using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transaction
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public DateTime Created { get; set; }

        public UserViewModel Client { get; set; }
        public ProductViewModel AccountFrom { get; set; }
        public ProductViewModel AccountTo { get; set; }
    }
}
