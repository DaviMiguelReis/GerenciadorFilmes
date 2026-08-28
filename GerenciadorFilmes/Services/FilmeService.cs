
using GerenciadorFilmes.Models;
using GerenciadorFilmes.ViewModels;


namespace GerenciadorFilmes.Services
{
    public class FilmeService : IFilmesServices
    {
        private readonly List<Genero> _generos =
    [
        new Genero
        {
            Id = 1,
            Nome = "Ação",
            
        },

        new Genero
        {
            Id = 2,
            Nome = "Terror",
           
        },

        new Genero
        {
            Id = 3,
            Nome = "Animação",
           
        }
    ];

        private readonly List<Filmes> _filmes =
        [
            new Filmes
        {
            Id = 1,
            Titulo = "Rambo",
            DuracaoMinutos = 100,
            AnoLancamento = 1993,
            GeneroId = 1

        },
        new Filmes
        {
            Id = 2,
            Titulo = "Vingadores",
            DuracaoMinutos = 120,
            AnoLancamento = 2012,
            GeneroId = 2

        },
        new Filmes
        {
            Id = 3,
            Titulo = "Homem-Aranha",
            DuracaoMinutos = 120,
            AnoLancamento = 2000,
            GeneroId = 3
        }


            ];
        public List<Filmes> Listar()
        {
            return _filmes
            .Select(VincularGenero)
            .ToList();
        }

        public Filmes? ObterPorId(int id)
        {
            var filme = _filmes.FirstOrDefault(p => p.Id == id);

            if (filme == null)
                return null;

            return VincularGenero(filme);
        }

        public void Adicionar(NovoFilmeViewModel vm)
        {
            var genero = _generos.FirstOrDefault(p => p.Id == vm.GeneroId);

            var novoFilme = new Filmes
            {
                Id = GerarNovoId(),
                Titulo = vm.Titulo,
                DuracaoMinutos = vm.DuracaoMinutos,
                AnoLancamento = vm.AnoLancamento,
                GeneroId = vm.GeneroId!.Value,
                Genero = genero
            };

            _filmes.Add(novoFilme);
        }

        public bool Atualizar(EditarFilmeViewModel model)
        {
            var filme = ObterPorId(model.Id);

            if (filme is null)
                return false;

            var professor = _generos.FirstOrDefault(p => p.Id == model.GeneroId);

            filme.Titulo = model.Titulo;
            filme.DuracaoMinutos = model.DuracaoMinutos;
            filme.AnoLancamento = model.AnoLancamento;
            filme.GeneroId = model.GeneroId!.Value;
            filme.Genero = professor;

            return true;
        }

        public bool Remover(int id)
        {
            var filme = ObterPorId(id);

            if (filme is null)
                return false;

            _filmes.Remove(filme);
            return true;
        }
        private int GerarNovoId()
        {
            return _filmes.Count == 0 ? 1 : _filmes.Max(filme => filme.Id) + 1;
        }



        public List<Filmes> PesquisarPorTitulo(string? titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return Listar();

            return _filmes
                .Where(filme => filme.Titulo.Contains(titulo, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        public List<Filmes> Ordenar(IEnumerable<Filmes> filmes, string? ordenarPor)
        {
            return ordenarPor?.ToLowerInvariant() switch
            {
                "titulo" => filmes.OrderBy(filme => filme.Titulo).ToList(),
                "AnoLacamento" => filmes.OrderBy(filme => filme.AnoLancamento).ToList(),
                _ => filmes.ToList()
            };
        }

        public List<Genero> ListarGeneros()
        {
            return _generos;
        }

        private Genero? ObterGeneroPorId(int generoId)
        {
            return _generos.FirstOrDefault(p => p.Id == generoId);
        }

        private Filmes VincularGenero(Filmes filme)
        {
            filme.Genero = ObterGeneroPorId(filme.GeneroId);

            return filme;
        }
    }
}
