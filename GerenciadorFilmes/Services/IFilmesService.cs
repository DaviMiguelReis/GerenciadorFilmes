using GerenciadorFilmes.Models;
using GerenciadorFilmes.ViewModels;

namespace GerenciadorFilmes.Services
{
    public interface IFilmeService
    {
        Task<List<Filmes>> ListarAsync();
        Task<List<Filmes>> PesquisarPorTituloAsync(string? titulo);
        List<Filmes> Ordenar(IEnumerable<Filmes> projetos, string? ordenarPor);
        Task<Filmes?> ObterPorIdAsync(int id);
        Task<List<Genero>> ListarGenerosAsync();
        Task AdicionarAsync(NovoFilmeViewModel model);
        Task<bool> AtualizarAsync(EditarFilmeViewModel model);
        Task<bool> RemoverAsync(int id);
    }
}
