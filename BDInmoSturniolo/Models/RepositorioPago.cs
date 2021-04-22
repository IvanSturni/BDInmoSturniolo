using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BDInmoSturniolo.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Pago p)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Pagos " +
                    $"({nameof(Pago.Fecha)}, {nameof(Pago.Importe)}, {nameof(Pago.ContratoId)}) " +
                    $"VALUES (@fecha, @importe, @contratoId);" +
                    "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fecha", p.Fecha);
                    command.Parameters.AddWithValue("@importe", p.Importe);
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
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
                string sql = $"DELETE FROM Pagos WHERE {nameof(Pago.Id)} = @id;";
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

        public int Modificacion(Pago p)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Pagos SET " +
                    $"{nameof(Pago.Fecha)}=@fecha, " +
                    $"{nameof(Pago.Importe)}=@importe, " +
                    $"{nameof(Pago.ContratoId)}=@contratoId " +
                    $"WHERE {nameof(Pago.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fecha", p.Fecha);
                    command.Parameters.AddWithValue("@importe", p.Importe);
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Pago Obtener(int id)
        {
            Pago res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Fecha)}, " +
                    $"{nameof(Pago.Importe)}, {nameof(Pago.ContratoId)}, " +
                    $"{nameof(Pago.Contrato.Inquilino.Apellido)}, {nameof(Pago.Contrato.Inquilino.Nombre)}, " +
                    $"{nameof(Pago.Contrato.Inmueble.Direccion)}, {nameof(Pago.Contrato.FechaInicio)} " +
                    $"FROM Pagos p " +
                    $"INNER JOIN Contratos c ON p.ContratoId = c.Id " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id " +
                    $"WHERE p.{nameof(Pago.Id)}=@id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Pago
                        {
                            Id = reader.GetInt32(0),
                            Fecha = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            ContratoId = reader.GetInt32(3),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(3),
                                Inquilino = new Inquilino
                                {
                                    Apellido = reader.GetString(4),
                                    Nombre = reader.GetString(5)
                                },
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(6)
                                },
                                FechaInicio = reader.GetDateTime(7)
                            }
                        };
                    }
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Pago> ObtenerPorContrato(int id)
        {
            IList<Pago> lista = new List<Pago>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Fecha)}, " +
                    $"{nameof(Pago.Importe)}, {nameof(Pago.ContratoId)}, " +
                    $"{nameof(Pago.Contrato.Inquilino.Apellido)}, {nameof(Pago.Contrato.Inquilino.Nombre)}, " +
                    $"{nameof(Pago.Contrato.Inmueble.Direccion)} " +
                    $"FROM Pagos p " +
                    $"INNER JOIN Contratos c ON p.ContratoId = c.Id " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id " +
                    $"WHERE p.{nameof(Pago.ContratoId)}=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Pago
                        {
                            Id = reader.GetInt32(0),
                            Fecha = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            ContratoId = reader.GetInt32(3),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(3),
                                Inquilino = new Inquilino
                                {
                                    Apellido = reader.GetString(4),
                                    Nombre = reader.GetString(5)
                                },
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(6)
                                }
                            }
                        });
                    }
                }
            }

            return lista;
        }

        public IList<Pago> ObtenerTodos()
        {
            IList<Pago> lista = new List<Pago>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Fecha)}, " +
                    $"{nameof(Pago.Importe)}, {nameof(Pago.ContratoId)}, " +
                    $"{nameof(Pago.Contrato.Inquilino.Apellido)}, {nameof(Pago.Contrato.Inquilino.Nombre)}, " +
                    $"{nameof(Pago.Contrato.Inmueble.Direccion)} " +
                    $"FROM Pagos p " +
                    $"INNER JOIN Contratos c ON p.ContratoId = c.Id " +
                    $"INNER JOIN Inquilinos inq ON c.InquilinoId = inq.Id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Pago
                        {
                            Id = reader.GetInt32(0),
                            Fecha = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            ContratoId = reader.GetInt32(3),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(3),
                                Inquilino = new Inquilino
                                {
                                    Apellido = reader.GetString(4),
                                    Nombre = reader.GetString(5)
                                },
                                Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(6)
                                }
                            }
                        });
                    }
                }
            }

            return lista;
        }
    }
}
