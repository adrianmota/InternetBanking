using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int Type { get; set; }
        public double? Limit { get; set; }
        public string ClientId { get; set; }
    }
}
