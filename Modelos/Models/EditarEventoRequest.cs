using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Models
{
    public class EditarEventoRequest
    {
        public int IdEvento { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        public int CapacidadMaxima { get; set; }
    }
}
