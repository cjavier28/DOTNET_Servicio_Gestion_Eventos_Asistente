using System;
using System.Collections.Generic;

namespace DOTNET_Servicio_Gestion_Eventos_Asistentes.Models;

public partial class UsuariosUsu
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string CorreoUsuario { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int UsuarioCreacion { get; set; }

    public int? UsuarioActualizacion { get; set; }

    public virtual ICollection<GestionEventosEve> GestionEventosEves { get; set; } = new List<GestionEventosEve>();

    public virtual ICollection<HistorialModificacionEve> HistorialModificacionEves { get; set; } = new List<HistorialModificacionEve>();

    public virtual ICollection<InscripcionesEvt> InscripcionesEvts { get; set; } = new List<InscripcionesEvt>();
}
