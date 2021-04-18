using System.Collections.Generic;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> ObtenerPorContrato(int id);
    }
}