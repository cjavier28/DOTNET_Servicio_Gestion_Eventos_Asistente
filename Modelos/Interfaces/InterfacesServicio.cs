using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
namespace Modelos.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<GestionEventosEve>> GetAllEventsAsync();
        Task<GestionEventosEve> CreateEventAsync(EventoLista EventoDatos);
        Task<UsuariosDatosGenerales> GetUserByIdAsync(int id);
        Task<UsuariosDatosGenerales> CreateUserAsync(UserDatos userDatos);
    }
}
