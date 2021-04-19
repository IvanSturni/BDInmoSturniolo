using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public enum usoInmueble
    {
        Comercial = 1,
        Residencial = 2
    }

    public class Inmueble
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }
        [Required]
        public int Uso { get; set; }
        public string UsoNombre => ((usoInmueble)Uso).ToString();
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        public int Superficie { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }
        [DisplayName("Disponible")]
        public bool EsDisponible { get; set; } = true;
        [DisplayName("Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio { get; set; }

        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnumUso = typeof(usoInmueble);
            foreach (var valor in Enum.GetValues(tipoEnumUso))
            {
                usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor));
            }
            return usos;
        }
    }
}
