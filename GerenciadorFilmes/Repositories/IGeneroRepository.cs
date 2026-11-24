using GerenciadorFilmes.Models;

namespace GerenciadorFilmes.Repositories
{
    public interface IGeneroRepository
    {
        Task<List<Genero>> ListarAsync();
        Task<Genero?> ObterPorIdAsync(int id);
    }
}
