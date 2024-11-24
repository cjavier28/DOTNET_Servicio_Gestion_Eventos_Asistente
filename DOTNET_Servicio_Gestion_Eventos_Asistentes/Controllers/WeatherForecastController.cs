using Microsoft.AspNetCore.Mvc;
using AccesoDatos;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{



    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherForecastController> _logger;
   
        // Constructor con inyección de dependencias
        public WeatherForecastController(ApplicationDbContext context, IConfiguration configuration, ILogger<WeatherForecastController> logger)
        {
            _context = context;  // Inyectando ApplicationDbContext
            _configuration = configuration;     
        }



  

        

        [HttpGet]
        [Route("[action]")]
        public async Task<string> GetWeatherForecast()
        {

            var salida = _context.GestionEventosEves.ToList(); ;
            return "";
           
        }
    }
}
