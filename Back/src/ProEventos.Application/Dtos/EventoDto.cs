using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime? DataEvento { get; set; }
        
        [Required(ErrorMessage = "o campo {0} é obrigatório."),
        //MinLength(3, ErrorMessage ="{0} deve ter no minimo 4 caracteres"),
        //MaxLength(50, ErrorMessage ="{0} deve ter no máximo 50 caracteres")
        StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido de 3 a 50 caracteres")]
        public string Tema { get; set; }

        [Display (Name="Quantidade de Pessoas")]
        [Range(1, 120000, ErrorMessage ="{0} não pode ser menor que 1 e maior que 120.000")]
        public int QtdPessoas { get; set; }
        [RegularExpression(@".*\.(gif|jpe?g|bpm|png)$", 
        ErrorMessage = "Não é uma imagem válida. (gif,jpg, jpeg, bpm ou png)")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório")]
        [Phone(ErrorMessage ="O {0} não é válido")]
        public string Telefone { get; set; }
        
        [Required(ErrorMessage ="O campo {0} é obrigatório.")]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "É necessário ser um {0} válido")]
        public string Email { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> PalestrantesEvenvtos { get; set; }
    }
}