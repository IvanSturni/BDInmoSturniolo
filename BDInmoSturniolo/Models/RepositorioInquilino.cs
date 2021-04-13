using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioInquilino : RepositorioBase
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inquilino i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Inquilinos " +
                    $"({nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, " +
                    $"{nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)}, " +
                    $"{nameof(Inquilino.Email)}) " +
                    $"VALUES (@nombre, @apellido, @dni, @telefono, @email);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
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
                string sql = $"DELETE FROM Inquilinos WHERE {nameof(Inquilino.Id)} = @id;";
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

        public int Modificacion(Inquilino i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inquilinos SET " +
                    $"{nameof(Inquilino.Nombre)}=@nombre, " +
                    $"{nameof(Inquilino.Apellido)}=@apellido, " +
                    $"{nameof(Inquilino.Dni)}=@dni, " +
                    $"{nameof(Inquilino.Telefono)}=@telefono, " +
                    $"{nameof(Inquilino.Email)}=@email " +
                    $"WHERE {nameof(Inquilino.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    command.Parameters.AddWithValue("@id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inquilino Obtener(int id)
        {
            Inquilino res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, " +
                    $"{nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)} " +
                    $"FROM Inquilinos WHERE {nameof(Inquilino.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5)
                        };
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> lista = new List<Inquilino>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, " +
                    $"{nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)} " +
                    $"FROM Inquilinos;";
                using (SqlCommand command = new SqlCommand(sql,connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                        });
                    }
                }
            }

            return lista;
        }
    }
}
