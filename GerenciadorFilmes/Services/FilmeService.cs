using GerenciadorFilmes.Models;
using GerenciadorFilmes.Repositories;
using GerenciadorFilmes.ViewModels;
using static System.Net.WebRequestMethods;

namespace GerenciadorFilmes.Services
{
    public class FilmeService : IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;

        public FilmeService(IFilmeRepository filmeRepository, IGeneroRepository generoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
        }

        public async Task<List<Genero>> ListarGenerosAsync() => await _generoRepository.ListarAsync();

        public async Task<List<Filmes>> ListarAsync()
        {
            var filmes = await _filmeRepository.ListarAsync();
            return await VincularGeneroAsync(filmes);
        }

        public async Task<List<Filmes>> PesquisarPorTituloAsync(string? titulo)
        {
            var filmes = await ListarAsync();
            if (string.IsNullOrWhiteSpace(titulo)) return filmes;

            return filmes.Where(filme => filme.Titulo.Contains(titulo, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public List<Filmes> Ordenar(IEnumerable<Filmes> filmes, string? ordenarPor) => ordenarPor?.ToLowerInvariant() switch
        {
            "titulo" => filmes.OrderBy(filme => filme.Titulo).ToList(),
            "Ano" => filmes.OrderBy(filme => filme.AnoLancamento).ToList(),
            _ => filmes.ToList()
        };

        public async Task<Filmes?> ObterPorIdAsync(int id)
        {
            var filme = await _filmeRepository.ObterPorIdAsync(id);
            if (filme is not null)
                filme.Genero = await _generoRepository.ObterPorIdAsync(filme.GeneroId);
            return filme;
        }

        public async Task AdicionarAsync(NovoFilmeViewModel model)
        {
            if (await _generoRepository.ObterPorIdAsync(model.GeneroId.Value) is null) // 
                throw new InvalidOperationException("O Gênero selecionado não existe.");

            await _filmeRepository.AdicionarAsync(new Filmes
            {
                Titulo = model.Titulo,
                DuracaoMinutos = model.DuracaoMinutos,
                AnoLancamento = model.AnoLancamento,
                GeneroId = model.GeneroId.Value // 
            });
        }

        public async Task<bool> AtualizarAsync(EditarFilmeViewModel model)
        {
            if (await _generoRepository.ObterPorIdAsync(model.GeneroId.Value) is null) return false; //

            return await _filmeRepository.AtualizarAsync(new Filmes
            {
                Id = model.Id,
                Titulo = model.Titulo,
                DuracaoMinutos = model.DuracaoMinutos,
                AnoLancamento = model.AnoLancamento,
                GeneroId = model.GeneroId.Value //
            });
        }

        public Task<bool> RemoverAsync(int id) => _filmeRepository.RemoverAsync(id);

        private async Task<List<Filmes>> VincularGeneroAsync(List<Filmes> filmes)
        {
            var generosPorId = (await _generoRepository.ListarAsync()).ToDictionary(genero => genero.Id);
            foreach (var filme in filmes)
                filme.Genero = generosPorId.GetValueOrDefault(filme.GeneroId);
            return filmes;
        }
    }
}