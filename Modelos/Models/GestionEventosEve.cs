using System;
using System.Collections.Generic;

namespace Modelos.Models;

public partial class GestionEventosEve
{
    public int IdEvento { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaHora { get; set; }

    public string Ubicacion { get; set; } = null!;

    public int CapacidadMaxima { get; set; }

    public int IdUsuario { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int UsuarioCreacion { get; set; }

    public int? UsuarioActualizacion { get; set; }

    public virtual ICollection<HistorialModificacionEve> HistorialModificacionEves { get; set; } = new List<HistorialModificacionEve>();

    public virtual UsuariosDatosGenerales IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<InscripcionesEvt> InscripcionesEvts { get; set; } = new List<InscripcionesEvt>();
}
