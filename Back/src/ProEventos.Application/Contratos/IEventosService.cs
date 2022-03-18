using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        public Task<EventoDto> AddEventos(int userId, EventoDto model);
        public Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        public Task<bool> DeleteEventos(int userId, int eventoId);
        public Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        public Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
        public Task<EventoDto[]> GetEventoByTemaAsync(int userId, string tema, bool includePalestrantes = false);
    }
}