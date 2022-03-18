using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersit
    {
        //EVENTOS
        Task<Evento[]> GetEventoByTemaAsync(int userId, string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}