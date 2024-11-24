using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public interface IEventoService
    {
        Task<EventoDTO> CrearEventoAsync(EventoDTO eventoDTO);
        Task<EventoDTO> EditarEventoAsync(int id, EventoDTO eventoDTO);
        Task<bool> EliminarEventoAsync(int id);
    }
}
