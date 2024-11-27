using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modelos.Models;
using Seguridad;
using Seguridad.Interfaces;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AccesoDatos.Servicios
{
    public class DataServiceADO
    {
        private readonly string _connectionString;
        private EncryptionService _encryptionService;
        // Constructor para inyectar la configuración de la conexión
        public DataServiceADO(IConfiguration configuration, IEncryptionService encryptionService)
        {
            _encryptionService = (EncryptionService)encryptionService;
            _connectionString = _encryptionService.Decrypt(configuration!.GetConnectionString("ConexionMensajeriaEscritura")!) ?? string.Empty;
        }

        // Crear un evento (Método asincrónico)
        public async Task<int?> CrearEventoAsync(CrearEventoRequest crearEventoRequest)
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
                    return (int?)outputIdEvento.Value;
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
        public async Task<int?> InscribirUsuarioEventoAsync(InscribirEventoRequest inscribirUsuarioEventoRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("paInscribirEvento", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Evento", inscribirUsuarioEventoRequest.IdEvento);
                cmd.Parameters.AddWithValue("@Id_Usuario", inscribirUsuarioEventoRequest.IdUsuario);

                // Definimos el parámetro de salida
                SqlParameter outputIdInscripcion = new SqlParameter("@Id_Inscripcion", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Agregar el parámetro de salida al comando
                cmd.Parameters.Add(outputIdInscripcion);

                try
                {
                    // Abrir la conexión y ejecutar el comando
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Verificar si el valor del parámetro de salida es DBNull
                    if (outputIdInscripcion.Value == DBNull.Value)
                    {
                        // Si es DBNull, devolver null
                        return null;
                    }
                    else
                    {
                        // Si tiene un valor, devolverlo como Nullable<int>
                        return (int?)outputIdInscripcion.Value;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return -1;
                }
            }
        }


        /// <summary>
        /// Se obtienen usuarios con  evento
        /// </summary>
        /// <returns></returns>
        public async Task<List<InformacionEvento>> ObtenerInformacionEvento(int idusuario)
        {
            var eventos = new List<InformacionEvento>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerUsuariosConEventos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@IdUsuario", idusuario);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            InformacionEvento evento = new InformacionEvento();

                            evento.IdEvento = reader.GetInt32(reader.GetOrdinal("IDEVENTO"));
                            evento.NombreEvento = reader.GetString(reader.GetOrdinal("NOMBRE_EVENTO"));
                            evento.Descripcion = reader.GetString(reader.GetOrdinal("DESCRIPCION"));
                            evento.FechaHora = reader.GetDateTime(reader.GetOrdinal("FECHAHORA"));
                            evento.Ubicacion = reader.GetString(reader.GetOrdinal("UBICACION"));
                            evento.CapacidadMaxima = reader.GetInt32(reader.GetOrdinal("CAPACIDADMAXIMA"));
                            evento.EstadoEvento = (bool)reader["ESTADOEVENTO"];
                            evento.TotalUsuarios = reader.GetInt32(reader.GetOrdinal("TOTAL"));
                            evento.IdUsuario = reader.GetInt32(reader.GetOrdinal("IDUSUARIO"));
                            evento.EstaInscrito = (int)reader["VALIDACION"];
                            eventos.Add(evento);
                        }
                    }
                }
            }

            return eventos;
        }

        /// <summary>
        /// Inserta usuario que se esta registrando
        /// </summary>
        /// <param name="usuarioGestionEventos"></param>
        /// <returns></returns>
        public async Task<int> InsertarUsuarioGestion(UsuarioGestionEventos usuarioGestionEventos)
        {
            // Crear la conexión con la base de datos
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    // Abrir la conexión
                    await conn.OpenAsync();

                    // Crear el comando para ejecutar el procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("paInsertarUsuario", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        cmd.Parameters.AddWithValue("@Nombre_Usuario", usuarioGestionEventos.Nombre_Usuario);
                        cmd.Parameters.AddWithValue("@Correo_Usuario", usuarioGestionEventos.Correo_Usuario);
                        cmd.Parameters.AddWithValue("@CnameUsuario", usuarioGestionEventos.CnameUsuario);
                        cmd.Parameters.AddWithValue("@ClaveUsuario", usuarioGestionEventos.ClaveUsuario);
                        cmd.Parameters.AddWithValue("@Estado", usuarioGestionEventos.Estado);
                        cmd.Parameters.AddWithValue("@UsuarioCreacion", usuarioGestionEventos.UsuarioCreacion);

                        SqlParameter outputParam = new SqlParameter("@Id_Usuario", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // Ejecutar el comando
                        await cmd.ExecuteNonQueryAsync();

                        // Obtener el valor del parámetro de salida
                        return (int)outputParam.Value;  // Devolver el ID generado
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error al insertar el usuario: " + ex.Message);
                    return 0;  // Retorna 0 si hay error
                }
            }
        }


        /// <summary>
        ///  Método asincrónico para verificar si el nombre de usuario ya está registrado
        /// </summary>
        /// <param name="cnameUsuario"></param>
        /// <param name="claveUsuario"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public async Task<int> ValidarUsuarioRegistradoAsync(UsuarioConsulta usuarioConsulta)
        {
            // Usamos 'using' para asegurarnos de que la conexión se cierre correctamente
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {

                    await conn.OpenAsync();


                    using (SqlCommand cmd = new SqlCommand("paValidarUsuarioRegistrado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@CnameUsuario", usuarioConsulta.Correo_Usuario);
                        cmd.Parameters.AddWithValue("@ClaveUsuario", usuarioConsulta.ClaveUsuario);


                        var resultado = await cmd.ExecuteScalarAsync();

                        // Retornar el resultado de la consulta (1 si existe, 0 si no existe)
                        return Convert.ToInt32(resultado);
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error al verificar usuario: {ex.Message}");
                    return -1;
                }
            }
        }


    }
}


