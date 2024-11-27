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
using Seguridad.Interfaces;

namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ApplicationEFDbContext _context;
        private readonly IEventoService _eventoNegocio;
        private readonly IEventoNegocioEf _eventoNegocioEf;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionService _encryptionService;

        public UserController(IEventoService eventoNegocio,
                                                ApplicationEFDbContext context,
                                                IConfiguration configuration,
                                                 IEventoNegocioEf eventoNegocioEf,
                                                 IEncryptionService encryptionService
            )
        {
            _context = context;
            _configuration = configuration;
            _eventoNegocio = eventoNegocio;
            _eventoNegocioEf = eventoNegocioEf;
            _encryptionService = encryptionService; 
        }

        // POST: api/evento/crear
        [HttpPost("registrarusuario")]
        public async Task<string> RegistrarUsuario([FromBody] UsuarioGestionEventos usuarioGestionEventos)
        {
            string respuesta = string.Empty;

            usuarioGestionEventos.ClaveUsuario = _encryptionService.EncriptarContrasena(usuarioGestionEventos.ClaveUsuario);

            try
            {
                int salida = await _eventoNegocio.InsertarUsuarioGestion(usuarioGestionEventos);
                if (salida == 0)
                {
                    respuesta = "Usuario no registrado";
                }
                else
                {
                    respuesta = "Usuario registrado exitosamente";
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta;
            }

        }

        [HttpPost]
        [Route("LoginUsuario")]
        public async Task<string> LoginUsuario(UsuarioGestionEventos usuarioGestionEventos)
        {
            string respuestatoken = string.Empty;   
            try
            {

             int salida =  await _eventoNegocio.ValidarUsuarioGestion(usuarioGestionEventos);
                if (salida == 0)
                {
                    respuestatoken = "Usuario no registrado";
                }
                else
                {
                    respuestatoken = _encryptionService.GenerarJWT(usuarioGestionEventos);
                }    
               
            return respuestatoken;
            }
            catch (Exception ex)
            {

                return respuestatoken;
            }

          
        }


        [HttpGet]
        [Route("ValidarToken")]
        public async Task<IActionResult> ValidarToken([FromQuery] string token)
        {
            bool esTokenValido = _encryptionService.ValidarToken(token);
            return StatusCode(200, new { isSuccess = esTokenValido });
        }

    }
}
