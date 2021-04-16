using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public interface IRepositorio<T>
    {
		int Alta(T p);
		int Baja(int id);
		int Modificacion(T p);
		T Obtener(int id);
		IList<T> ObtenerTodos();
	}
}
