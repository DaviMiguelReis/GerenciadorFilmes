using GerenciadorFilmes.Models;

namespace GerenciadorFilmes.Repositories
{
    public class GeneroJsonRepository : IGeneroRepository
    {
        private const string NomeArquivo = "generos.json";
        private readonly JsonFileStore _arquivo;

        public GeneroJsonRepository(JsonFileStore arquivo) => _arquivo = arquivo;

        public Task<List<Genero>> ListarAsync() => _arquivo.LerAsync<Genero>(NomeArquivo);

        public async Task<Genero?> ObterPorIdAsync(int id) =>
            (await ListarAsync()).FirstOrDefault(genero => genero.Id == id);
    }
}
