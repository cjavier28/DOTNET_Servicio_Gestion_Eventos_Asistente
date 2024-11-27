using System;
using System.Collections.Generic;


namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Controllers
{
    public class LoginRequest
    {
        public string Usuario { get; set; } = string.Empty;
        public string Clave { get; set; }= string.Empty;
    }
}
