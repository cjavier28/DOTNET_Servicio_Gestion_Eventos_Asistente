using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public interface IEventoNegocioEf
    {
        /// <summary>
        /// Lista todos los eventos.
        /// </summary>
        /// <returns>Lista de eventos.</returns>
        List<GestionEventosEve> ListarEventos();

        /// <summary>
        /// Lista un evento específico por su ID.
        /// </summary>
        /// <param name="idEvento">ID del evento.</param>
        /// <returns>Lista de eventos que coinciden con el ID.</returns>
        List<GestionEventosEve> ListarEventoPorId(int idEvento);
    }
}
