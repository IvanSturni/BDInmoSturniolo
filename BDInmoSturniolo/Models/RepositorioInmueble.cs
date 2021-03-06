using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inmueble ent)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Inmuebles " +
                    $"({nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, " +
                    $"{nameof(Inmueble.PropietarioId)}) " +
                    $"VALUES (@descripcion, @uso, @tipo, @dni, @superficie, @direccion, @precio, @esDisponible, @propietarioId);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", ent.Descripcion);
                    command.Parameters.AddWithValue("@uso", ent.Uso);
                    command.Parameters.AddWithValue("@tipo", ent.Tipo);
                    command.Parameters.AddWithValue("@dni", ent.Ambientes);
                    command.Parameters.AddWithValue("@superficie", ent.Superficie);
                    command.Parameters.AddWithValue("@direccion", ent.Direccion);
                    command.Parameters.AddWithValue("@precio", ent.Precio);
                    command.Parameters.AddWithValue("@esDisponible", ent.EsDisponible ? 1 : 0);
                    command.Parameters.AddWithValue("@propietarioId", ent.PropietarioId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    ent.Id = res;
                    connection.Close();
                }

            }
            return ent.Id;
        }

        public int Baja(int id)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inmuebles WHERE {nameof(Inmueble.Id)} = @id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            return res;
        }

        public int Modificacion(Inmueble ent)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inmuebles SET " +
                    $"{nameof(Inmueble.Descripcion)}=@descripcion, " +
                    $"{nameof(Inmueble.Uso)}=@uso, " +
                    $"{nameof(Inmueble.Tipo)}=@tipo, " +
                    $"{nameof(Inmueble.Ambientes)}=@dni, " +
                    $"{nameof(Inmueble.Superficie)}=@superficie, " +
                    $"{nameof(Inmueble.Direccion)}=@direccion, " +
                    $"{nameof(Inmueble.Precio)}=@precio, " +
                    $"{nameof(Inmueble.EsDisponible)}=@esDisponible, " +
                    $"{nameof(Inmueble.PropietarioId)}=@propietarioId " +
                    $"WHERE {nameof(Inmueble.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", ent.Id);
                    command.Parameters.AddWithValue("@descripcion", ent.Descripcion);
                    command.Parameters.AddWithValue("@uso", ent.Uso);
                    command.Parameters.AddWithValue("@tipo", ent.Tipo);
                    command.Parameters.AddWithValue("@dni", ent.Ambientes);
                    command.Parameters.AddWithValue("@superficie", ent.Superficie);
                    command.Parameters.AddWithValue("@direccion", ent.Direccion);
                    command.Parameters.AddWithValue("@precio", ent.Precio);
                    command.Parameters.AddWithValue("@esDisponible", ent.EsDisponible ? 1 : 0);
                    command.Parameters.AddWithValue("@propietarioId", ent.PropietarioId);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble Obtener(int id)
        {
            Inmueble res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, {nameof(Inmueble.PropietarioId)}, " +
                    $"{nameof(Inmueble.Duenio.Nombre)}, {nameof(Inmueble.Duenio.Apellido)}, {nameof(Inmueble.Duenio.Dni)} " +
                    $"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id " +
                    $"WHERE i.{nameof(Inmueble.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Superficie = reader.GetInt32(5),
                            Direccion = reader.GetString(6),
                            Precio = reader.GetDecimal(7),
                            EsDisponible = reader.GetByte(8) == 1,
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                                Dni = reader.GetString(12)
                            }
                        };
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Inmueble> ObtenerPorPropietario(int id)
        {
            IList<Inmueble> lista = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, {nameof(Inmueble.PropietarioId)}, " +
                    $"{nameof(Inmueble.Duenio.Nombre)}, {nameof(Inmueble.Duenio.Apellido)}, {nameof(Inmueble.Duenio.Dni)} " +
                    $"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id " +
                    $"WHERE i.PropietarioId = @id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Superficie = reader.GetInt32(5),
                            Direccion = reader.GetString(6),
                            Precio = reader.GetDecimal(7),
                            EsDisponible = reader.GetByte(8) == 1,
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                                Dni = reader.GetString(12)
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public IList<Inmueble> ObtenerDisponibles()
        {
            IList<Inmueble> lista = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, {nameof(Inmueble.PropietarioId)}, " +
                    $"{nameof(Inmueble.Duenio.Nombre)}, {nameof(Inmueble.Duenio.Apellido)}, {nameof(Inmueble.Duenio.Dni)} " +
                    $"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id " +
                    $"WHERE i.EsDisponible = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Superficie = reader.GetInt32(5),
                            Direccion = reader.GetString(6),
                            Precio = reader.GetDecimal(7),
                            EsDisponible = reader.GetByte(8) == 1,
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                                Dni = reader.GetString(12)
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public IList<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fi, DateTime ff, int IdInm = 0)
        {
            IList<Inmueble> lista = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, {nameof(Inmueble.PropietarioId)}, " +
                    $"{nameof(Inmueble.Duenio.Nombre)}, {nameof(Inmueble.Duenio.Apellido)}, {nameof(Inmueble.Duenio.Dni)} " +
                    $"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id " +
                    $"WHERE i.EsDisponible = 1 " +
                    $"AND i.{nameof(Inmueble.Id)} NOT IN " +
                        $"(SELECT {nameof(Contrato.InmuebleId)} FROM Contratos " +
                        $"WHERE (@fi >= FechaInicio AND @fi < ISNULL(FechaCancelacion, FechaFinal)) " +
                        $"OR (@ff > FechaInicio AND @ff <= ISNULL(FechaCancelacion, FechaFinal)) " +
                        $"OR (@fi <= FechaInicio AND @ff >= ISNULL(FechaCancelacion, FechaFinal))) ";
                if (IdInm > 0)
                    sql += $"AND i.{nameof(Inmueble.Id)} = @IdInm ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fi", fi);
                    command.Parameters.AddWithValue("@ff", ff);
                    if (IdInm > 0)
                        command.Parameters.AddWithValue("@IdInm", IdInm);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Superficie = reader.GetInt32(5),
                            Direccion = reader.GetString(6),
                            Precio = reader.GetDecimal(7),
                            EsDisponible = reader.GetByte(8) == 1,
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                                Dni = reader.GetString(12)
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> lista = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.EsDisponible)}, {nameof(Inmueble.PropietarioId)}, " +
                    $"{nameof(Inmueble.Duenio.Nombre)}, {nameof(Inmueble.Duenio.Apellido)}, {nameof(Inmueble.Duenio.Dni)} " +
                    $"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Superficie = reader.GetInt32(5),
                            Direccion = reader.GetString(6),
                            Precio = reader.GetDecimal(7),
                            EsDisponible = reader.GetByte(8) == 1,
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                                Dni = reader.GetString(12)
                            }
                        });
                    }
                }
            }

            return lista;
        }
    }
}
