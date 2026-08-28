using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GerenciadorFilmes.ViewModels
{
    public class FilmeFormularioViewModel
    {
        [Required(ErrorMessage = "Informe o título do filme.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a duração do filme.")]
        [Display(Name = "Duracao")]
        public int DuracaoMinutos { get; set; } 

        [Range(1895, 2049, ErrorMessage = "Informe o Ano de lançamento.")]
        [Display(Name = "Ano lançamento")]
        public int AnoLancamento { get; set; }

        [Required(ErrorMessage = "Selecione um Gênero.")]
        [Display(Name = "Gênero")]
        public int? GeneroId { get; set; }

        public List<SelectListItem> Generos { get; set; } = [];
    }
}
