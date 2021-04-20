using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
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
                    $"({nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFinal)}, {nameof(Contrato.Monto)}, " +
                    $"{nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}) " +
                    $"VALUES (@fechaInicio, @fechaFinal, @monto, @inquilinoId, @inmuebleId);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", modelo.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", modelo.FechaFinal);
                    command.Parameters.AddWithValue("@monto", modelo.Monto);
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

        public int Cancelar(int id)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Contratos SET " +
                    $"{nameof(Contrato.FechaCancelacion)}=@fechaCancelacion " +
                    $"WHERE {nameof(Contrato.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@fechaCancelacion", DateTime.Now);
                    var comando = command.Transaction;
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
                    $"{nameof(Contrato.FechaCancelacion)}= @fechaCancelacion, " +
                    $"{nameof(Contrato.Monto)}= @monto, " +
                    $"{nameof(Contrato.InquilinoId)}=@inquilinoId, " +
                    $"{nameof(Contrato.InmuebleId)}=@inmuebleId " +
                    $"WHERE {nameof(Contrato.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", modelo.Id);
                    command.Parameters.AddWithValue("@fechaInicio", modelo.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFinal", modelo.FechaFinal);
                    command.Parameters.AddWithValue("@fechaCancelacion", modelo.FechaCancelacion.HasValue ? modelo.FechaCancelacion : DBNull.Value);
                    command.Parameters.AddWithValue("@monto", modelo.Monto);
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
                    $"{nameof(Contrato.FechaCancelacion)}, {nameof(Contrato.Monto)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, " +
                    $"{nameof(Contrato.Inquilino.Nombre)}, {nameof(Contrato.Inquilino.Apellido)}, {nameof(Contrato.Inmueble.Direccion)}, " +
                    $"{nameof(Contrato.Inmueble.Precio)} " +
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
                            FechaCancelacion = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            Monto = reader.GetDecimal(4),
                            InquilinoId = reader.GetInt32(5),
                            InmuebleId = reader.GetInt32(6),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8)
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(6),
                                Direccion = reader.GetString(9),
                                Precio = reader.GetDecimal(10)
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
                    $"{nameof(Contrato.FechaCancelacion)}, {nameof(Contrato.Monto)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, " +
                    $"{nameof(Contrato.Inquilino.Nombre)}, {nameof(Contrato.Inquilino.Apellido)}, {nameof(Contrato.Inmueble.Direccion)}, " +
                    $"{nameof(Contrato.Inmueble.Precio)} " +
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
                            FechaCancelacion = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            Monto = reader.GetDecimal(4),
                            InquilinoId = reader.GetInt32(5),
                            InmuebleId = reader.GetInt32(6),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8)
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(6),
                                Direccion = reader.GetString(9),
                                Precio = reader.GetDecimal(10)
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public IList<Contrato> ObtenerVigentes()
        {
            IList<Contrato> lista = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFinal)}, " +
                    $"{nameof(Contrato.FechaCancelacion)}, {nameof(Contrato.Monto)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, " +
                    $"{nameof(Contrato.Inquilino.Nombre)}, {nameof(Contrato.Inquilino.Apellido)}, {nameof(Contrato.Inmueble.Direccion)}, " +
                    $"{nameof(Contrato.Inmueble.Precio)} " +
                    $"FROM Contratos c " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato x = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetDateTime(1),
                            FechaFinal = reader.GetDateTime(2),
                            FechaCancelacion = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            Monto = reader.GetDecimal(4),
                            InquilinoId = reader.GetInt32(5),
                            InmuebleId = reader.GetInt32(6),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8)
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(6),
                                Direccion = reader.GetString(9),
                                Precio = reader.GetDecimal(10)
                            }
                        };
                        if (x.EsVigente)
                            lista.Add(x);
                    }
                }
            }

            return lista;
        }
    }
}
