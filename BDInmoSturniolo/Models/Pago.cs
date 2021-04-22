using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Importe { get; set; }
        [DisplayName("Número de Pago")]
        public int NumeroPago => 1 + Fecha.Month - (Contrato != null ? Contrato.FechaInicio.Month : Fecha.Month) +
            (Fecha.Year - (Contrato != null ? Contrato.FechaInicio.Year : Fecha.Year)) * 12;
        [Required, DisplayName("Contrato")]
        public int ContratoId { get; set; }
        [ForeignKey(nameof(ContratoId))]
        public Contrato Contrato { get; set; }
    }
}
