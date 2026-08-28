using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GerenciadorFilmes.Services;
using GerenciadorFilmes.ViewModels;

namespace GerenciadorFilmes.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IFilmesServices _filmeService;

        public FilmesController(IFilmesServices filmeService)
        {
            _filmeService = filmeService;
        }

        private List<SelectListItem> ObterGeneroSelectList()
        {
            return _filmeService.ListarGeneros()
                .Select(genero => new SelectListItem
                {
                    Value = genero.Id.ToString(),
                    Text = genero.Nome
                })
                .ToList();
        }



        public IActionResult Index(string? pesquisa, string? ordenarPor)
        {
            var filmes = _filmeService.PesquisarPorTitulo(pesquisa);

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


        public IActionResult Detalhes(int id)
        {
            var filme = _filmeService.ObterPorId(id);

            if (filme is null)
                return NotFound();

            return View(filme);
        }



        [HttpGet]
        public IActionResult Cadastrar()
        {
            var model = new NovoFilmeViewModel
            {
                Generos = ObterGeneroSelectList()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(NovoFilmeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                model.Generos = ObterGeneroSelectList();

                return View(model);
            }

            _filmeService.Adicionar(model);

            TempData["Mensagem"] = "Filme cadastrado com sucesso!";

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Editar(int id)
        {
            var filme = _filmeService.ObterPorId(id);

            if (filme is null)
                return NotFound();

            var model = new EditarFilmeViewModel
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                DuracaoMinutos = filme.DuracaoMinutos,
                AnoLancamento = filme.AnoLancamento,

             
                GeneroId = filme.GeneroId,

                
                Generos = ObterGeneroSelectList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(EditarFilmeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                model.Generos = ObterGeneroSelectList();

                return View(model);
            }

            var atualizado = _filmeService.Atualizar(model);

            if (!atualizado)
                return NotFound();

            TempData["Mensagem"] = "Projeto atualizado com sucesso!";

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var filme = _filmeService.ObterPorId(id);

            if (filme is null)
                return NotFound();

            return View(filme);
        }



        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExclusao(int id)
        {
            var removido = _filmeService.Remover(id);

            if (!removido)
                return NotFound();

            TempData["Mensagem"] = "Projeto excluído com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
