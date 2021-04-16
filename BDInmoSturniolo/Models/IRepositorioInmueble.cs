using System.Collections.Generic;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerPorPropietario(int id);
    }
}