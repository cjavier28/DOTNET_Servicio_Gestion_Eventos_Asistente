using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Modelos.Models;
using Seguridad;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AccesoDatos.Servicios
{
    public class DataServiceADO
    {
        private readonly string _connectionString;
        private EncryptionService _encryptionService = new();
        // Constructor para inyectar la configuración de la conexión
        public DataServiceADO(IConfiguration configuration)
        {
            _connectionString = _encryptionService.Decrypt(configuration!.GetConnectionString("ConexionMensajeriaEscritura")!) ?? string.Empty;
        }

        // Crear un evento (Método asincrónico)
        public async Task<int> CrearEventoAsync(CrearEventoRequest crearEventoRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("paCrearEvento", connection);
                cmd.CommandType = CommandType.StoredProcedure;

             
                cmd.Parameters.AddWithValue("@Nombre", crearEventoRequest.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", crearEventoRequest.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Hora", crearEventoRequest.FechaHora);
                cmd.Parameters.AddWithValue("@Ubicacion", crearEventoRequest.Ubicacion);
                cmd.Parameters.AddWithValue("@Capacidad_Maxima", crearEventoRequest.CapacidadMaxima);
                cmd.Parameters.AddWithValue("@Id_Usuario", crearEventoRequest.IdUsuario);

               
                SqlParameter outputIdEvento = new SqlParameter("@Id_Evento", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdEvento);

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (int)outputIdEvento.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return -1;
                }
            }
        }

        // Editar un evento (Método asincrónico)
        public async Task<int> EditarEventoAsync(EditarEventoRequest editarEventoRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("paEditarEvento", connection);
                cmd.CommandType = CommandType.StoredProcedure;

            
                cmd.Parameters.AddWithValue("@Id_Evento", editarEventoRequest.IdEvento);
                cmd.Parameters.AddWithValue("@Id_Usuario", editarEventoRequest.IdUsuario);
                cmd.Parameters.AddWithValue("@Fecha_Hora", editarEventoRequest.FechaHora);
                cmd.Parameters.AddWithValue("@Ubicacion", editarEventoRequest.Ubicacion);
                cmd.Parameters.AddWithValue("@Capacidad_Maxima", editarEventoRequest.CapacidadMaxima);

             
                SqlParameter outputIdEvento = new SqlParameter("@Id_Evento_Salida", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdEvento);

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (int)outputIdEvento.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return -1;
                }
            }
        }

        // Eliminar un evento (Método asincrónico)
        public async Task<bool> EliminarEventoAsync(EliminarEventoRequest eliminarEventoRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("paEliminarEvento", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@Id_Evento", eliminarEventoRequest.IdEvento);
                cmd.Parameters.AddWithValue("@Id_Usuario", eliminarEventoRequest.IdUsuario);

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        // Inscribir un usuario a un evento (Método asincrónico)
        public async Task<int> InscribirUsuarioEventoAsync(InscribirEventoRequest inscribirUsuarioEventoRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("paInscribirEvento", connection);
                cmd.CommandType = CommandType.StoredProcedure;

            
                cmd.Parameters.AddWithValue("@Id_Evento", inscribirUsuarioEventoRequest.IdEvento);
                cmd.Parameters.AddWithValue("@Id_Usuario", inscribirUsuarioEventoRequest.IdUsuario);

               
                SqlParameter outputIdInscripcion = new SqlParameter("@Id_Inscripcion", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdInscripcion);

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (int)outputIdInscripcion.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return -1;
                }
            }
        }
    }
}
