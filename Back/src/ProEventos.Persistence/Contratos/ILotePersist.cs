using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersit
    {
        /// <summary>
        /// Método get que retornará uma lista de lotes por evento
        /// </summary>
        /// <param name="eventoId">Chave evento</param>
        /// <returns>Array de lotes</returns>
        Task<Lote[]> GetAllLotesByEventoIdAsync(int eventoId);
        /// <summary>
        /// Método get que retornará apenas 1 lote
        /// </summary>
        /// <param name="eventoId">Chave Id</param>
        /// <param name="loteId">Chave lote</param>
        /// <returns>Apenas 1 lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}