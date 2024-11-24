using Modelos;

namespace Negocio
{
    public class EventoServiceEF : IEventoService
    {
        public Task<EventoDTO> CrearEventoAsync(EventoDTO eventoDTO)
        {
            throw new NotImplementedException();
        }

        public Task<EventoDTO> EditarEventoAsync(int id, EventoDTO eventoDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarEventoAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
