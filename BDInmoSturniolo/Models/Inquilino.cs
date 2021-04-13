using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class Inquilino
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [DisplayName("DNI")]
        public string Dni { get; set; }
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
