using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoAnderson2.Data;
using ProjetoAnderson2.Models;
using System.Diagnostics;
using System.IO;

namespace ProjetoAnderson2.Controllers
{
    public class HomeController : Controller
    {
        private readonly LocadoraContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, LocadoraContext context)
        {
            _context = context;
            _logger = logger;
        }

        public TopFilmes BuscarUltimosFilmes(int quantidade)
        {
            if (!_context.Filme.Any())
            {
                return new TopFilmes();
            }

            var ultimosFilmes = _context.Filme
                   .OrderByDescending(f => f.Id)
                   .Take(quantidade)
                   .ToList();
            TopFilmes resultado = new TopFilmes { Filmes = ultimosFilmes };
            return resultado;
        }

        public IActionResult Index()
        {
            var resultado = BuscarUltimosFilmes(10);
            return View(resultado);
        }

        public TopMensagens OrdenarMensagem()
        {
            var ultimasMensagens = _context.Contato
                   .OrderByDescending(f => f.Id)
                   .ToList();
            TopMensagens resultado = new TopMensagens { Contatos = ultimasMensagens };
            return resultado;
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult PgContato()
        {
            var resultado = OrdenarMensagem();
            return View(resultado);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Filme == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .Include(f => f.Artista)
                .Include(f => f.Genero)
                .Include(f => f.Situacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // GET: Filmes/Create
        public IActionResult Create()
        {
            ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome");
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Nome");
            ViewData["SituacaoId"] = new SelectList(_context.Situacao, "Id", "Status");
            return View();
        }

        //DATAILS DA PAGINA DE CONTATOS
        public async Task<IActionResult> DetailsContato(int? id)
        {
            if (id == null || _context.Contato == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }


    }
}