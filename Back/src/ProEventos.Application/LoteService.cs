using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class LoteService : ILotesService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersit _lotePersit;
        private readonly IMapper _mapper;
        public LoteService(IGeralPersist geralPersist, ILotePersit lotePersit, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _lotePersit = lotePersit;
            _mapper = mapper;

        }
         public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                _geralPersist.Add<Lote>(lote);
                await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]>SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersit.GetAllLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;
                foreach (var model in models)
                {
                    if(model.Id == 0){
                        await AddLote(eventoId, model);
                    }else{
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, lote);
                        _geralPersist.Update<Lote>(lote);
                        await _geralPersist.SaveChangesAsync();
                    }
                }
                var loteRetorno = await _lotePersit.GetAllLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersit.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote a ser deletado não foi encontrado.");
                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> GetAllLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersit.GetAllLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;
                var resultado = _mapper.Map<LoteDto[]>(lotes);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<LoteDto>GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersit.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return null;
                var resultado = _mapper.Map<LoteDto>(lote);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}