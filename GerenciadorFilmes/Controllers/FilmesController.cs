using GerenciadorFilmes.Models;
using GerenciadorFilmes.Services;
using GerenciadorFilmes.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorFilmes.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IFilmeService _filmeService;

        public FilmesController(IFilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        public async Task<IActionResult> Index(string? pesquisa, string? ordenarPor)
        {
            var filmes = await _filmeService.PesquisarPorTituloAsync(pesquisa);
            filmes = _filmeService.Ordenar(filmes, ordenarPor);

            var model = new FilmesIndexViewModels
            {
                filmes = filmes,
                TextoPesquisa = pesquisa,
                QuantidadeTotal = filmes.Count,
                OrdenarPor = ordenarPor
            };

            return View(model);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var filme = await _filmeService.ObterPorIdAsync(id);

            if (filme is null)
                return NotFound();

            return View(filme);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar()
        {
            var model = new NovoFilmeViewModel
            {
                Generos = await ObtersGeneroslectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(NovoFilmeViewModel model)
        {
            model.Generos = await ObtersGeneroslectListAsync();

            if (!ModelState.IsValid)
                return View(model);

            await _filmeService.AdicionarAsync(model);
            TempData["Mensagem"] = "Filme cadastrado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var filme = await _filmeService.ObterPorIdAsync(id);

            if (filme is null)
                return NotFound();

            var model = new EditarFilmeViewModel
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                DuracaoMinutos = filme.DuracaoMinutos,
                AnoLancamento = filme.AnoLancamento,
                GeneroId = filme.GeneroId,
                Generos = await ObtersGeneroslectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarFilmeViewModel model)
        {
            model.Generos = await ObtersGeneroslectListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var atualizado = await _filmeService.AtualizarAsync(model);

            if (!atualizado)
                return NotFound();

            TempData["Mensagem"] = "Projeto atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var filme = await _filmeService.ObterPorIdAsync(id);

            if (filme is null)
                return NotFound();

            return View(filme);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var removido = await _filmeService.RemoverAsync(id);

            if (!removido)
                return NotFound();

            TempData["Mensagem"] = "Projeto excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> ObtersGeneroslectListAsync()
        {
            return (await _filmeService.ListarGenerosAsync())
                .Select(genero => new SelectListItem
                {
                    Value = genero.Id.ToString(),
                    Text = genero.Nome
                })
                .ToList();
        }
    }
}