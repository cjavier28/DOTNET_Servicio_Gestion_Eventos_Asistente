using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Models
{
    public class InformacionEvento
    {
        public int IdEvento { get; set; }
        public string NombreEvento { get; set; }=string.Empty;
        public string Descripcion { get; set; } = string.Empty; 
        public DateTime FechaHora { get; set; } 
        public string Ubicacion { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public bool EstadoEvento { get; set; } 
        public int TotalUsuarios { get; set; }
    }
}
