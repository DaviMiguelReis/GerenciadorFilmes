using GerenciadorFilmes.Models;

namespace GerenciadorFilmes.Repositories
{
    public interface IFilmeRepository
    {
        Task<List<Filmes>> ListarAsync();
        Task<Filmes?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Filmes filme);
        Task<bool> AtualizarAsync(Filmes filme);
        Task<bool> RemoverAsync(int id);
    }
}
