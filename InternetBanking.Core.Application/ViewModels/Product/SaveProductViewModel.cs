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
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de Producto")]
        [DataType(DataType.Currency)]
        public int Type { get; set; }

        [DataType(DataType.Currency)]
        public double? Limit { get; set; }
        public string ClientId { get; set; }
    }
}
