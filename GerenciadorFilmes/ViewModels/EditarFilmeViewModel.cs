using GerenciadorFilmes.Models;
using System.ComponentModel.DataAnnotations;


namespace GerenciadorFilmes.ViewModels
{
    public class EditarFilmeViewModel : FilmeFormularioViewModel
    {
        public int Id { get; set; }
    }
}
