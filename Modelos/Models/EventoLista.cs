namespace Modelos.Models
{
    public class EventoLista
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
    }
}
