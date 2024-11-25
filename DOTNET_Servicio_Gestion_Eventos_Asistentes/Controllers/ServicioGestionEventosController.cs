using Microsoft.AspNetCore.Mvc;
using AccesoDatos;
using AccesoDatos.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modelos;
using Modelos.Models;
using Negocio;
using AccesoDatos.Contexto;

namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ServicioGestionEventosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventoService _eventoNegocio;  // Cambiar a IEventoService
        private readonly IConfiguration _configuration;

        // Constructor con inyección de dependencias
        public ServicioGestionEventosController(IEventoService eventoNegocio,  // Cambiar a IEventoService
                                                ApplicationDbContext context,
                                                IConfiguration configuration)
        {
            _context = context;  // Inyectando ApplicationDbContext
            _configuration = configuration;
            _eventoNegocio = eventoNegocio;  // Asignar la interfaz en lugar de la clase directamente
        }

        // POST: api/evento/crear
        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento([FromBody] CrearEventoRequest crearEventoRequest)
        {
            try
            {
                int idEvento = await _eventoNegocio.CrearEventoAsync(crearEventoRequest);
                if (idEvento > 0)
                {
                    return Ok(new { IdEvento = idEvento }); // Retorna el ID del evento creado
                }
                return BadRequest("Error al crear el evento");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // PUT: api/evento/editar
        [HttpPut("editar")]
        public async Task<IActionResult> EditarEvento([FromBody] EditarEventoRequest editarEventoRequest)
        {
            try
            {
                int idEvento = await _eventoNegocio.EditarEventoAsync(editarEventoRequest);
                if (idEvento > 0)
                {
                    return Ok(new { IdEvento = idEvento }); // Retorna el ID del evento editado
                }
                return BadRequest("Error al editar el evento");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: api/evento/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarEvento([FromBody] EliminarEventoRequest eliminarEventoRequest)
        {
            try
            {
                bool eliminado = await _eventoNegocio.EliminarEventoAsync(eliminarEventoRequest);
                if (eliminado)
                {
                    return Ok("Evento eliminado con éxito");
                }
                return BadRequest("Error al eliminar el evento");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: api/evento/inscribir
        [HttpPost("inscribir")]
        public async Task<IActionResult> InscribirUsuarioEvento([FromBody] InscribirEventoRequest inscribirEventoRequest)
        {
            try
            {
                int idInscripcion = await _eventoNegocio.InscribirUsuarioEventoAsync(inscribirEventoRequest);
                if (idInscripcion > 0)
                {
                    return Ok(new { IdInscripcion = idInscripcion }); // Retorna el ID de la inscripción
                }
                return BadRequest("Error al inscribir al usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
