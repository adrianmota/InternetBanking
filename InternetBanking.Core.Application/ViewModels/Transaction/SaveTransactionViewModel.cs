using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transaction
{
    public class SaveTransactionViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }

        [Required(ErrorMessage ="Debe ingresar un monto para transferir")]
        public double Amount { get; set; }
        public string ClientId { get; set; }
        public int AccountFromId { get; set; }
        public int AccountToId { get; set; }
    }
}
