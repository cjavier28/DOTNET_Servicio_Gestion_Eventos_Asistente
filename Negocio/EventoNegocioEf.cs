using AccesoDatos.Contexto;
using AccesoDatos.Servicios;
using Modelos.Models;
using Negocio.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EventoNegocioEf: IEventoNegocioEf
    {
   
        private readonly ApplicationEFDbContext _context;
        // Constructor para inyectar el servicio de acceso a datos
        public EventoNegocioEf(ApplicationEFDbContext context) {
            _context = context;
        } 

        /// <summary>
        /// Lista Eventos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  List<GestionEventosEve> ListarEventos()
        {
            List<GestionEventosEve> lstGestionEventosEve = new();
            try
            {
                lstGestionEventosEve =  _context.GestionEventosEves.ToList();   
                return lstGestionEventosEve;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar eventos:{JsonConvert.SerializeObject(ex)} " );
            }
          
        }

        /// <summary>
        /// Lista evento por ID
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<GestionEventosEve> ListarEventoPorId(int idEvento)
        {
            List<GestionEventosEve> lstGestionEventosEve = new();
            try
            {
                lstGestionEventosEve = _context.GestionEventosEves.Where(x=>x.IdEvento== idEvento).ToList();
                return lstGestionEventosEve;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar eventos:{JsonConvert.SerializeObject(ex)} ");
            }

        }
    }
}
