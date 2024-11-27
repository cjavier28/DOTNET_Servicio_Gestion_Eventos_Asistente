namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Models
{
    public class UsuarioEvento
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string CorreoUsuario { get; set; } = string.Empty;
        public bool EstadoUsuario { get; set; }
        public int IdEvento { get; set; }
        public string NombreEvento { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public bool EstadoEvento { get; set; }
    }
}
