using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IEnumerable<Evento> _evento = new Evento[]{
            new Evento(){
                    EventoId = 1,
                    Tema = "Cursão Angular",
                    Local = "São Paulo",
                    Lote = "1° Lote",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                    ImagemURL = "foto.png"
              },
              new Evento(){
                    EventoId = 2,
                    Tema = "Cursão brabo",
                    Local = "São Paulo",
                    Lote = "2° Lote",
                    QtdPessoas = 600,
                    DataEvento = DateTime.Now.AddDays(5).ToString("dd/MM/yyyy"),
                    ImagemURL = "benis.png"
              }
        };
        public EventoController()
        {
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _evento;
        }
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _evento.Where(evento => evento.EventoId == id);
        }
        [HttpPost]
        public string Post()
        {
            return "Post";
        }
        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"Put com id = {id}";
        }
        [HttpDelete("{id}")]
        public string delete(int id)
        {
             return $"Delete com id = {id}";
        }
        
    }
}
