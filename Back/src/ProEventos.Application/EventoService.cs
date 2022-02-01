using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersit _eventoPersit;
        public EventoService(IGeralPersist geralPersist, IEventoPersit eventoPersit)
        {
            _eventoPersit = eventoPersit;
            _geralPersist = geralPersist;

        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralPersist.add<Evento>(model);
                if(await _geralPersist.SaveChangesAsync()){
                    return await _eventoPersit.GetEventoByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
               var evento = await _eventoPersit.GetEventoByIdAsync(eventoId, false);
               if(evento == null) return null;
               model.Id = evento.Id;
               _geralPersist.Update(model);
               if(await _geralPersist.SaveChangesAsync()){
                   return await _eventoPersit.GetEventoByIdAsync(model.Id, false);
               }
               return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEventos(int eventoId)
        {
            try
            {
               var evento = await _eventoPersit.GetEventoByIdAsync(eventoId, false);
               if(evento == null) throw new Exception("Evento a ser deletado não foi encontrado.");
               _geralPersist.Delete<Evento>(evento);
               return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersit.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) return null;
                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento[]> GetEventoByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersit.GetEventoByTemaAsync(tema, includePalestrantes);
                if(eventos == null) return null;
                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersit.GetEventoByIdAsync(eventoId, includePalestrantes);
                if(eventos == null) return null;
                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}