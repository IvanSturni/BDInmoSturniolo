using System;
using System.Collections.Generic;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerDisponibles();
        IList<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fi, DateTime ff);
        IList<Inmueble> ObtenerPorPropietario(int id);
    }
}