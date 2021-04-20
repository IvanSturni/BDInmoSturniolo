using System.Collections.Generic;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        int Cancelar(int id);
        IList<Contrato> ObtenerPorInmueble(int id);
        IList<Contrato> ObtenerVigentes();
    }
}