using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface ILotesService
    {
        public Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models);
        public Task<bool> DeleteLote(int eventoId, int loteId);

        public Task<LoteDto[]> GetAllLotesByEventoIdAsync(int eventoId);
        public Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}