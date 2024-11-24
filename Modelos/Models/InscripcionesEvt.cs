using System;
using System.Collections.Generic;

namespace Modelos.Models;

public partial class InscripcionesEvt
{
    public int IdInscripcion { get; set; }

    public int IdEvento { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaInscripcion { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int UsuarioCreacion { get; set; }

    public int? UsuarioActualizacion { get; set; }

    public virtual GestionEventosEve IdEventoNavigation { get; set; } = null!;

    public virtual UsuariosUsu IdUsuarioNavigation { get; set; } = null!;
}
