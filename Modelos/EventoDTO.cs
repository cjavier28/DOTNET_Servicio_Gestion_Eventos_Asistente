namespace Modelos
{
    public class EventoDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; } =string.Empty;
        public int CapacidadMaxima { get; set; }
        public int IdUsuario { get; set; }
    }
}
