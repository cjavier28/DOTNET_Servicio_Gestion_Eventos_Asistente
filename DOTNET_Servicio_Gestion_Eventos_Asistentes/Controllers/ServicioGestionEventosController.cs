using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Contexto;
using Modelos.Models;
using Newtonsoft.Json;
using Negocio.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using Modelos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;



namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ServicioGestionEventosController : ControllerBase
    {
        private readonly ApplicationEFDbContext _context;
        private readonly IEventoService _eventoNegocio;
        private readonly IEventoNegocioEf _eventoNegocioEf;
        private readonly IConfiguration _configuration;


        public ServicioGestionEventosController(IEventoService eventoNegocio,
                                                ApplicationEFDbContext context,
                                                IConfiguration configuration,
                                                 IEventoNegocioEf eventoNegocioEf
            )
        {
            _context = context;
            _configuration = configuration;
            _eventoNegocio = eventoNegocio;
            _eventoNegocioEf = eventoNegocioEf;
        }

        // POST: api/evento/crear
        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento([FromBody] CrearEventoRequest crearEventoRequest)
        {
            try
            {
                int? idEvento = await _eventoNegocio.CrearEventoAsync(crearEventoRequest);
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
                int? idInscripcion = await _eventoNegocio.InscribirUsuarioEventoAsync(inscribirEventoRequest);
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

        [HttpPost("obtener")]
        public async Task<IActionResult> ObtenerInformacionEvento()
        {
            try
            {
                List<InformacionEvento> lstInformacionEvento = await _eventoNegocio.ObtenerInformacionEvento();


                return Ok(lstInformacionEvento); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {JsonConvert.SerializeObject(ex)}");
            }
        }


        /// <summary>
        /// Obtiene todos los eventos.
        /// </summary>
        /// <returns>Lista de eventos.</returns>
        [HttpGet("ListarEventos")]
        public ActionResult<List<GestionEventosEve>> GetEventos()
        {
            try
            {
                var eventos = _eventoNegocioEf.ListarEventos();
                if (eventos == null || !eventos.Any())
                {
                    return NotFound("No se encontraron eventos.");
                }
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        public class LoginRequest
        {
            public string Usuario { get; set; }= string.Empty;
            public string Clave { get; set; } = string.Empty;
        }

       
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Usuario == "admin" && loginRequest.Clave == "123") // Ejemplo: Valida con tu lógica
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, loginRequest.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1qKJ6V91qsmLmrw9lT7Rz0z+FRh7mvbggHfMoqm9oA4="));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "https://mi-servidor-autenticacion.com",
                    audience: "https://mi-api.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    

    /// <summary>
    /// Obtiene un evento por su ID.
    /// </summary>
    /// <param name="id">ID del evento.</param>
    /// <returns>Evento correspondiente al ID.</returns>
    [HttpGet("{id:int}")]
        public ActionResult<GestionEventosEve> GetEventoById(int id)
        {
            try
            {
                var eventos = _eventoNegocioEf.ListarEventoPorId(id);
                if (eventos == null || !eventos.Any())
                {
                    return NotFound($"No se encontró el evento con ID {id}.");
                }
                return Ok(eventos.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
