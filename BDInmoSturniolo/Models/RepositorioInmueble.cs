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

        public int Alta(Inmueble i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Inmuebles " +
                    $"({nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, " +
                    $"{nameof(Inmueble.PropietarioId)}) " +
                    $"VALUES (@descripcion, @tipo, @dni, @superficie, @direccion, @precio, @propietarioId);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", i.Descripcion);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@dni", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }

            }
            return i.Id;
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

        public int Modificacion(Inmueble i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inmuebles SET " +
                    $"{nameof(Inmueble.Descripcion)}=@descripcion, " +
                    $"{nameof(Inmueble.Tipo)}=@tipo, " +
                    $"{nameof(Inmueble.Ambientes)}=@dni, " +
                    $"{nameof(Inmueble.Superficie)}=@superficie, " +
                    $"{nameof(Inmueble.Direccion)}=@direccion, " +
                    $"{nameof(Inmueble.Precio)}=@precio, " +
                    $"{nameof(Inmueble.PropietarioId)}=@propietarioId " +
                    $"WHERE {nameof(Inmueble.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", i.Id);
                    command.Parameters.AddWithValue("@descripcion", i.Descripcion);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@dni", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
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
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.PropietarioId)}, " +
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
                            Tipo = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Superficie = reader.GetInt32(4),
                            Direccion = reader.GetString(5),
                            Precio = reader.GetDecimal(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                                Dni = reader.GetString(10)
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
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.PropietarioId)}, " +
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
                            Tipo = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Superficie = reader.GetInt32(4),
                            Direccion = reader.GetString(5),
                            Precio = reader.GetDecimal(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                                Dni = reader.GetString(10)
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
                string sql = $"SELECT i.{nameof(Inmueble.Id)}, {nameof(Inmueble.Descripcion)}, {nameof(Inmueble.Tipo)}, " +
                    $"{nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Superficie)}, " +
                    $"{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.PropietarioId)}, " +
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
                            Tipo = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Superficie = reader.GetInt32(4),
                            Direccion = reader.GetString(5),
                            Precio = reader.GetDecimal(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                                Dni = reader.GetString(10)
                            }
                        });
                    }
                }
            }

            return lista;
        }
    }
}
