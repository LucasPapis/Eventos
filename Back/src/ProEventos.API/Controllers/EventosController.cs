using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventosService;
        private readonly IWebHostEnvironment _hostEnvironment;
         private readonly IAccountService _accountService;

        public EventosController(IEventosService eventosService, IWebHostEnvironment hostEnvironment,
               IAccountService accountService)
        {
            _accountService = accountService;
            _hostEnvironment = hostEnvironment;
            _eventosService = eventosService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventosService.GetAllEventosAsync(User.GetUserId(),true);
                if (eventos == null) return NoContent();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }
        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var evento = await _eventosService.GetEventoByTemaAsync(User.GetUserId(), tema, true);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventosService.AddEventos(User.GetUserId(), model);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }
                var EventoRetorno = await _eventosService.UpdateEvento(User.GetUserId(), eventoId, evento);
                return Ok(EventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventosService.UpdateEvento(User.GetUserId(), id, model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar o evento.");
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar alterar eventos. Erro: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();
                if (await _eventosService.DeleteEventos(User.GetUserId(), id))
                {
                    DeleteImage(evento.ImagemURL);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Erro não especificado ao tentar deletar");
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10)
            .ToArray()).Replace(' ', '-');
            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssffff")}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

    }
}
