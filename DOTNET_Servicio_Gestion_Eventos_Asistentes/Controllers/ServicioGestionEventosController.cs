using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Contexto;
using Modelos;
using Modelos.Models;
using Newtonsoft.Json;

namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ServicioGestionEventosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventoService _eventoNegocio;  
        private readonly IConfiguration _configuration;

      
        public ServicioGestionEventosController(IEventoService eventoNegocio,  
                                                ApplicationDbContext context,
                                                IConfiguration configuration)
        {
            _context = context;  
            _configuration = configuration;
            _eventoNegocio = eventoNegocio; 
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
                    return Ok(new { IdEvento = idEvento }); 
                }
                return BadRequest("Error al crear el evento");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {JsonConvert.SerializeObject(ex)}");
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
                    return Ok(new { IdEvento = idEvento }); 
                }
                return BadRequest("Error al editar el evento");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {JsonConvert.SerializeObject(ex)}");
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
                return StatusCode(500, $"Error interno: {JsonConvert.SerializeObject(ex)}");
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
                    return Ok(new { IdInscripcion = idInscripcion });
                }
                return BadRequest("Error al inscribir al usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}
