using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class Contrato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Fecha Inicio"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DisplayName("Fecha Final"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; set; }
        [DisplayName("Inquilino")]
        public int InquilinoId { get; set; }
        [DisplayName("Inmueble")]
        public int InmuebleId { get; set; }
        [ForeignKey(nameof(InquilinoId))]
        public Inquilino Inquilino { get; set; }
        [ForeignKey(nameof(InmuebleId))]
        public Inmueble Inmueble { get; set; }
    }
}
