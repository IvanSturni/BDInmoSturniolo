using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorio<Propietario>
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Propietario p)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Propietarios " +
                    $"({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, " +
                    $"{nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, " +
                    $"{nameof(Propietario.Email)}, {nameof(Propietario.Clave)}) " +
                    $"VALUES (@nombre, @apellido, @dni, @telefono, @email, @clave);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@clave", p.Clave);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
                
            }
            return p.Id;
        }

        public int Baja(int id)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Propietarios WHERE {nameof(Propietario.Id)} = @id;";
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

        public int Modificacion(Propietario p)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Propietarios SET " +
                    $"{nameof(Propietario.Nombre)}=@nombre, " +
                    $"{nameof(Propietario.Apellido)}=@apellido, " +
                    $"{nameof(Propietario.Dni)}=@dni, " +
                    $"{nameof(Propietario.Telefono)}=@telefono, " +
                    $"{nameof(Propietario.Email)}=@email, " +
                    $"{nameof(Propietario.Clave)}=@clave " +
                    $"WHERE {nameof(Propietario.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@clave", p.Clave);
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario Obtener(int id)
        {
            Propietario res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, " +
                    $"{nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, " +
                    $"{nameof(Propietario.Email)}, {nameof(Propietario.Clave)} " +
                    $"FROM Propietarios WHERE {nameof(Propietario.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6)
                        };
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> lista = new List<Propietario>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, " +
                    $"{nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, " +
                    $"{nameof(Propietario.Email)}, {nameof(Propietario.Clave)} " +
                    $"FROM Propietarios;";
                using (SqlCommand command = new SqlCommand(sql,connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
