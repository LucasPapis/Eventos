using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
        public Task<EventoDto> AddEventos(EventoDto model);
        public Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        public Task<bool> DeleteEventos(int eventoId);

        public Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
        public Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
        public Task<EventoDto[]> GetEventoByTemaAsync(string tema, bool includePalestrantes = false);
    }
}