using GerenciadorFilmes.Models;

namespace GerenciadorFilmes.Repositories
{
    public class FilmeJsonRepository : IFilmeRepository
    {
        private const string NomeArquivo = "filmes.json";
        private readonly JsonFileStore _arquivo;

        public FilmeJsonRepository(JsonFileStore arquivo) => _arquivo = arquivo;

        public Task<List<Filmes>> ListarAsync() => _arquivo.LerAsync<Filmes>(NomeArquivo);

        public async Task<Filmes?> ObterPorIdAsync(int id) =>
            (await ListarAsync()).FirstOrDefault(projeto => projeto.Id == id);

        public async Task AdicionarAsync(Filmes filme)
        {
            await _arquivo.AlterarAsync<Filmes, object?>(NomeArquivo, filmes =>
            {
                filme.Id = filmes.Count == 0 ? 1 : filmes.Max(item => item.Id) + 1;
                filme.Genero = null;
                filmes.Add(filme);
                return null;
            });
        }

        public Task<bool> AtualizarAsync(Filmes filme) => _arquivo.AlterarAsync<Filmes, bool>(NomeArquivo, filmes =>
        {
            var existente = filmes.FirstOrDefault(item => item.Id == filme.Id);
            if (existente is null)
                return false;

            existente.Titulo = filme.Titulo;
            existente.DuracaoMinutos = filme.DuracaoMinutos;
            existente.AnoLancamento = filme.AnoLancamento;
            existente.GeneroId = filme.GeneroId;
            return true;
        });

        public Task<bool> RemoverAsync(int id) => _arquivo.AlterarAsync<Filmes, bool>(NomeArquivo, filmes =>
        {
            return filmes.RemoveAll(filme => filme.Id == id) > 0;
        });
    }

}

