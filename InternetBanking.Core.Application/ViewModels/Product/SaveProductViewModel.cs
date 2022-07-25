using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocarle un monto al producto")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de Producto")]
        public int Type { get; set; }

        public double? Limit { get; set; }
        public string ClientId { get; set; }
    }
}
