using System;
using System.Collections.Generic;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerDisponibles();
        IList<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fi, DateTime ff, int IdInm = 0);
        IList<Inmueble> ObtenerPorPropietario(int id);
    }
}