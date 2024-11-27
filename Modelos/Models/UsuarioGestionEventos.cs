using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Models
{
    public partial class UsuarioGestionEventos
    {
        public int Id_Usuario { get; set; }  
        public string Nombre_Usuario { get; set; } 
        public string Correo_Usuario { get; set; }  
        public string CnameUsuario { get; set; }  
        public bool Estado { get; set; }  
        public DateTime FechaCreacion { get; set; }  
        public string ClaveUsuario { get; set; }  
        public DateTime? FechaActualizacion { get; set; }  
        public int UsuarioCreacion { get; set; } 
        public int UsuarioActualizacion { get; set; }  
    }
}
