using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft;


namespace DOTNET_Servicio_Gestion_Eventos_AsistenteTestNU
{
    public class ServicioGestionEventosTests
    {
        private readonly IConfiguration _configuration;
        public ServicioGestionEventosTests()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettingsTest.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            string cadena = _configuration!.GetSection("ConexionMensajeriaEscritura").Value ;
        }


        [Test]
        public void Test1()
        {
            bool Respuesta = true;
         
            Assert.True(Respuesta);
        }
    }
}