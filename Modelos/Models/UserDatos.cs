﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Models
{
    public class UserDatos
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;   
        public string CorreoUsuario { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
