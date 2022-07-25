using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Admin
{
    public class DashboardViewModel
    {
        public int TodayTransactions { get; set; }
        public double TodayTransactionsAmount { get; set; }
        public int TodayPays { get; set; }
        public double TodayPaysAmount { get; set; }
        public int TotalTransactions { get; set; }
        public double TotalTransactionsAmount { get; set; }
        public int TotalPays { get; set; }
        public double TotalPaysAmount { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int ProductsCount { get; set; }
    }
}
