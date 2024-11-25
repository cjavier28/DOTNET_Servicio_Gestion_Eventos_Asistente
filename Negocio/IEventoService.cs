using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos.Models;

namespace Modelos
{
    public interface IEventoService
    {
        // Método asincrónico para crear un evento
        Task<int> CrearEventoAsync(CrearEventoRequest crearEventoRequest);

        // Método asincrónico para editar un evento
        Task<int> EditarEventoAsync(EditarEventoRequest editarEventoRequest);

        // Método asincrónico para eliminar un evento
        Task<bool> EliminarEventoAsync(EliminarEventoRequest eliminarEventoRequest);

        // Método asincrónico para inscribir un usuario en un evento
        Task<int> InscribirUsuarioEventoAsync(InscribirEventoRequest inscribirUsuarioEventoRequest);
    }
}
