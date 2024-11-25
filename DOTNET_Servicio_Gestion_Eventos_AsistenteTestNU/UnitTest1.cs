using Microsoft.Extensions.Configuration;
using Newtonsoft;
using Negocio;
using AccesoDatos;
using AccesoDatos.Servicios;
using Seguridad;
using Seguridad.Interfaces;
using Negocio.Interfaces;
using Modelos.Models;

namespace DOTNET_Servicio_Gestion_Eventos_AsistenteTestNU
{
    public class ServicioGestionEventosTests
    {
        private readonly IConfiguration _configuration;
        private readonly IEncryptionService iencryptionService;
        private EventoNegocioAdo eventoNegocioAdo;
        private DataServiceADO dataServiceADO;
        private string cadena = string.Empty;
        public ServicioGestionEventosTests()
        {

            var builder = new ConfigurationBuilder().AddJsonFile("appsettingsTest.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            cadena = _configuration!.GetConnectionString("ConexionMensajeriaEscritura") ?? string.Empty;

            iencryptionService = new EncryptionService();
            dataServiceADO = new DataServiceADO(_configuration, iencryptionService);
            eventoNegocioAdo = new EventoNegocioAdo(dataServiceADO);
        }


        [Test]
        public async Task InscribirUsuarioEventoTest()
        {
            bool respuesta = true;
            InscribirEventoRequest inscribirEventoRequest = new()
            {
                IdEvento = 1,
                IdUsuario = 1,
            };
            int? respuestaInscripcion = await eventoNegocioAdo.InscribirUsuarioEventoAsync(inscribirEventoRequest);
            respuesta = respuestaInscripcion == null ? true : false;
            Assert.True(respuesta);
        }



        [Test]
        public async Task CrearEventoAsyncTest()
        {
            bool respuesta = true;
            CrearEventoRequest crearEventoRequest = new()
            {
                CapacidadMaxima = 1,
                Descripcion = "test",
                FechaHora = DateTime.Now,
                IdUsuario = 1,
                Nombre = "Nombre Test",
                Ubicacion = "Calle 8 19-27"
            };
            int? respuestaInscripcion = await eventoNegocioAdo.CrearEventoAsync(crearEventoRequest);
            respuesta = respuestaInscripcion > 0 ? true : false;
            Assert.True(respuesta);
        }



        [Test]
        public async Task EditarEventoAsync()
        {
            bool respuesta = true;
            EditarEventoRequest editarEventoRequest = new()
            {
                CapacidadMaxima = 1,
                IdEvento = 1,
                FechaHora = DateTime.Now,
                IdUsuario = 1,                
                Ubicacion = "Calle 8 19-27"
            };
            int? respuestaInscripcion = await eventoNegocioAdo.EditarEventoAsync(editarEventoRequest);
            respuesta = respuestaInscripcion > 0 ? true : false;
            Assert.True(respuesta);
        }
    }
}