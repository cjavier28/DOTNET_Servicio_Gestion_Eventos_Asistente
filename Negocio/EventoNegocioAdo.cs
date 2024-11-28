using AccesoDatos.Servicios;
using Modelos.Models;
using Negocio.Interfaces;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ServiceReference1;

namespace Negocio
{
    public class EventoNegocioAdo: IEventoService
    {
        private readonly DataServiceADO _dataService;

        // Constructor para inyectar el servicio de acceso a datos
        public EventoNegocioAdo(DataServiceADO dataService)
        {
            _dataService = dataService;
        }



        // Método para crear un evento
        public async Task<int?> CrearEventoAsync(CrearEventoRequest crearEventoRequest)
        {
            try
            {
                // Delegamos la creación del evento al servicio ADO
                int? idEvento = await _dataService.CrearEventoAsync(crearEventoRequest);

                if (idEvento > 0)
                {
                    return idEvento; // Retornamos el ID del evento creado
                }

                return -1; // Si hubo un error, retornamos -1
            }
            catch (Exception ex)
            {
                // Gestión de excepciones
                throw new Exception("Error en la capa de negocio al crear el evento: " +  ex.Message);
            }
        }

        // Método para editar un evento
        public async Task<int> EditarEventoAsync(EditarEventoRequest editarEventoRequest)
        {
            try
            {
                // Delegamos la edición del evento al servicio ADO
                int idEventoSalida = await _dataService.EditarEventoAsync(
                   editarEventoRequest
                );

                if (idEventoSalida > 0)
                {
                    return idEventoSalida; 
                }

                return -1; 
            }
            catch (Exception ex)
            {               
                throw new Exception("Error en la capa de negocio al editar el evento: " + ex.Message);
            }
        }

        // Método para inscribir un usuario en un evento
        public async Task<bool> EliminarEventoAsync(EliminarEventoRequest eliminarEventoRequest)
        {
            try
            {
                bool respuesta =  await _dataService.EliminarEventoAsync(eliminarEventoRequest);
                return respuesta;
            }
            catch (Exception ex)
            {
                // Gestión de excepciones
                throw new Exception("Error en la capa de negocio al eliminar el evento: " + ex.Message);
            }
        }


        /// <summary>
        /// Inscribe usuario a  evento
        /// </summary>
        /// <param name="inscribirUsuarioEventoRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int?> InscribirUsuarioEventoAsync(InscribirEventoRequest inscribirUsuarioEventoRequest)
        {
            try
            {
                return await _dataService.InscribirUsuarioEventoAsync(
                   inscribirUsuarioEventoRequest
                );
            }
            catch (Exception ex)
            {
                // Gestión de excepciones
                throw new Exception("Error en la capa de negocio al inscribir al usuario en el evento: " + ex.Message);
            }
        }

        /// <summary>
        /// Capa negocio: Obtener usuarios con evento
        /// </summary>
        /// <param name="inscribirUsuarioEventoRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<InformacionEvento>> ObtenerInformacionEvento(int idusuario)
        {
            try
            {
                return await _dataService.ObtenerInformacionEvento(idusuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al inscribir al usuario en el evento: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta  usuario para gestión
        /// </summary>
        /// <param name="usuarioGestionEventos"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> InsertarUsuarioGestion(UsuarioGestionEventos usuarioGestionEventos)
        {
            try
            {
                return await _dataService.InsertarUsuarioGestion(usuarioGestionEventos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la capa de negocio al insertar usuario {ex.Message} ");
            }
        }

        /// <summary>
        /// Valida que  exista usuario
        /// </summary>
        /// <param name="usuarioConsulta"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> ValidarUsuarioGestion(UsuarioConsulta usuarioConsulta)
        {
            try
            {
                return await _dataService.ValidarUsuarioRegistradoAsync(usuarioConsulta);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la capa de negocio al insertar usuario {ex.Message} ");
            }
        }

        public void EnviarMensajeProoveedor()
        {
            ServiceReference1.serviciowebPortTypeClient servicioMensajeria  = new serviciowebPortTypeClient();

            MensajeProveedor mensajeProveedor = new MensajeProveedor();
            servicioMensajeria.EnviarMensajeAsync(mensajeProveedor.Numero, mensajeProveedor.Mensaje2, mensajeProveedor.Nota, mensajeProveedor.Usuario, mensajeProveedor.Clave, mensajeProveedor.Programado);
        }
    }
}
