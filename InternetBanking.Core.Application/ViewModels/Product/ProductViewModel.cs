using InternetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public double? Limit { get; set; }

        public UserViewModel Client { get; set; }
    }
}
