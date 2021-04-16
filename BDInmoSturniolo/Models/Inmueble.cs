using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class Inmueble
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }
        // TODO: agregar uso
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        public int Superficie { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [DisplayName("Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio { get; set; }
    }
}
