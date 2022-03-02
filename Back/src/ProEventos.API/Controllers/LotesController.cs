using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Domain;
using ProEventos.Application.Contratos;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILotesService _loteService;
        public LotesController(ILotesService eventosService)
        {
            _loteService = eventosService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetAllLotesByEventoIdAsync(eventoId);
                if(lotes == null) return NoContent();
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lote = await _loteService.SaveLotes(eventoId, models);
                if(lote == null) return NoContent();
                return Ok(lote);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) return NoContent();
                return await _loteService.DeleteLote(lote.EventoId, lote.Id) ? 
                Ok(new {message = "Deletado"}) : throw new Exception("Erro não especificado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar atualizar lotes. Erro: {ex.Message}");
            }
        }

    }
}
