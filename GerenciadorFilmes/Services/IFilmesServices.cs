using GerenciadorFilmes.Models;
using GerenciadorFilmes.ViewModels;

namespace GerenciadorFilmes.Services
{
    public interface IFilmesServices
    {
        List<Filmes> Listar();
        Filmes? ObterPorId(int id);
        void Adicionar(NovoFilmeViewModel model);
        bool Atualizar(EditarFilmeViewModel model);
        bool Remover(int id);
        List<Filmes> PesquisarPorTitulo(string? titulo);
        List<Filmes> Ordenar(IEnumerable<Filmes> filmes, string? ordenarPor);
        List<Genero> ListarGeneros();
    }
}
