using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato modelo)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Contratos " +
                    $"({nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFinal)}, " +
                    $"{nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}) " +
                    $"VALUES (@fechaInicio, @fechaFinal, @inquilinoId, @inmuebleId);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", modelo.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", modelo.FechaFinal);
                    command.Parameters.AddWithValue("@inquilinoId", modelo.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", modelo.InmuebleId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    modelo.Id = res;
                    connection.Close();
                }
                
            }
            return modelo.Id;
        }

        public int Baja(int id)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Contratos WHERE {nameof(Contrato.Id)} = @id;";
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

        public int Modificacion(Contrato modelo)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Contratos SET " +
                    $"{nameof(Contrato.FechaInicio)}= @fechaInicio, " +
                    $"{nameof(Contrato.FechaFinal)}= @fechaFinal, " +
                    $"{nameof(Contrato.InquilinoId)}=@inquilinoId, " +
                    $"{nameof(Contrato.InmuebleId)}=@inmuebleId " +
                    $"WHERE {nameof(Contrato.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", modelo.Id);
                    command.Parameters.AddWithValue("@fechaInicio", modelo.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", modelo.FechaFinal);
                    command.Parameters.AddWithValue("@inquilinoId", modelo.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", modelo.InmuebleId);
                    var comando = command.Transaction;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato Obtener(int id)
        {
            Contrato res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFinal)}, " +
                    $"{nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, " +
                    $"{nameof(Contrato.Inquilino.Nombre)}, {nameof(Contrato.Inquilino.Apellido)}, {nameof(Contrato.Inmueble.Direccion)} " +
                    $"FROM Contratos c " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id " +
                    $"WHERE c.{nameof(Contrato.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetDateTime(1),
                            FechaFinal = reader.GetDateTime(2),
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6)
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(7)
                            }
                        };
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> lista = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFinal)}, " +
                    $"{nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, " +
                    $"{nameof(Contrato.Inquilino.Nombre)}, {nameof(Contrato.Inquilino.Apellido)}, {nameof(Contrato.Inmueble.Direccion)} " +
                    $"FROM Contratos c " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id;";
                using (SqlCommand command = new SqlCommand(sql,connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetDateTime(1),
                            FechaFinal = reader.GetDateTime(2),
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6)
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(7)
                            }
                        });
                    }
                }
            }

            return lista;
        }
    }
}
