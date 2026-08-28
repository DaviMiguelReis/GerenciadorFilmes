using GerenciadorFilmes.Models;

namespace GerenciadorFilmes.ViewModels
{
    public class FilmesIndexViewModels
    {
        public List<Filmes> filmes { get; set; } = new();

        public string? TextoPesquisa { get; set; }

        public int QuantidadeTotal { get; set; }

        public string? OrdenarPor { get; set; }
    }
}
