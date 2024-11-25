using System;
using System.Collections.Generic;

namespace Modelos.Models;

public partial class HistorialModificacionEve
{
    public int IdModificacion { get; set; }

    public int IdEvento { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string? DescripcionModificacion { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int UsuarioCreacion { get; set; }

    public int? UsuarioActualizacion { get; set; }

    public virtual GestionEventosEve IdEventoNavigation { get; set; } = null!;

    public virtual UsuariosDatosGenerales IdUsuarioNavigation { get; set; } = null!;
}
