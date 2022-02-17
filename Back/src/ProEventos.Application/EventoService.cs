using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersit _eventoPersit;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersist geralPersist, IEventoPersit eventoPersit, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersit = eventoPersit;
            _mapper = mapper;

        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var EventoRetorno = await _eventoPersit.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(EventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersit.GetEventoByIdAsync(eventoId, false);
                if (evento == null) return null;
                model.Id = evento.Id;
                _mapper.Map(model,evento);
                _geralPersist.Update<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var EventoRetorno = await _eventoPersit.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(EventoRetorno);
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
                if (evento == null) throw new Exception("Evento a ser deletado não foi encontrado.");
                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersit.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;
                var resultado = _mapper.Map<EventoDto[]>(eventos);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetEventoByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersit.GetEventoByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;
                var resultado = _mapper.Map<EventoDto[]>(eventos);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersit.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;
                var resultado = _mapper.Map<EventoDto>(evento);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}