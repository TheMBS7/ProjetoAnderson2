using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoAnderson2.Data;
using ProjetoAnderson2.Models;

namespace ProjetoAnderson2.Controllers
{
    public class FilmesController : Controller
    {
        private readonly LocadoraContext _context;
        private string _filePath;

        public FilmesController(LocadoraContext context, IWebHostEnvironment env)
        {
            _filePath = env.WebRootPath;
            _context = context;
        }

        // GET: Filmes
        // GET: Movies
        public async Task<IActionResult> Index(string? movieGenre, string title)
        {
            if (_context.Filme == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            // Use LINQ to get list of genres.
            //IQueryable<Genero> genreQuery = from m in _context.Filme
            //                               orderby m.GeneroId
            //                              select m.Generos;

            IQueryable<Genero> genreQuery = _context.Filme
                .OrderBy(x => x.GeneroId)
                .Select(x => x.Genero)
                .AsQueryable()!;

            IQueryable<Filme> movies = _context.Filme
                .Include(x => x.Genero)
                .Include(x => x.Artista)
                .Include(x => x.Situacao)
                .AsQueryable()!;

            if (!string.IsNullOrEmpty(title))
            {
                movies = movies.Where(s => s.Titulo!.Contains(title));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genero.Nome.Contains(movieGenre));
            }

            var filteredMovies = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Filmes = await movies.ToListAsync()
            };

            return View(filteredMovies);
        }

        
        // GET: Filmes/Details/5
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
        [Authorize]
        // GET: Filmes/Create
        public IActionResult Create()
        {
            ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome");
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Nome");
            ViewData["SituacaoId"] = new SelectList(_context.Situacao, "Id", "Status");
            return View();
        }

        [Authorize]
        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoDeBarras,Titulo,GeneroId,Ano,Tipo,Preco,DataAdquirida,ValorCusto,SituacaoId,ArtistaId,Diretor,Imagem")] Filme filme, IFormFile anexo)
        {
            ModelState.Remove("Artista");
            ModelState.Remove("Genero");
            ModelState.Remove("Situacao");
            ModelState.Remove("Imagem");

            if (ModelState.IsValid)
            {
                if (!ValidaImagem(anexo))
                    return View(filme);

                var nome = SalvarArquivo(anexo);
                filme.Imagem = nome;

                _context.Add(filme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome", filme.ArtistaId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Nome", filme.GeneroId);
            ViewData["SituacaoId"] = new SelectList(_context.Situacao, "Id", "Status", filme.SituacaoId);
            return View(filme);
        }

        //valida imagem
        public bool ValidaImagem(IFormFile anexo)
        {
            switch (anexo.ContentType)
            {
                case "image/jpeg":
                    return true;

                case "image/jpg":
                    return true;

                case "image/bmp":
                    return true;

                case "image/gif":
                    return true;

                case "image/png":
                    return true;

                default:
                    return false;
                    break;
            }
        }

        //salva imagem
        public string SalvarArquivo(IFormFile anexo)
        {
            var nome = Guid.NewGuid().ToString() + anexo.FileName;

            var filePath = _filePath + "\\fotos";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using (var stream = System.IO.File.Create(filePath + "\\" + nome))
            {
                anexo.CopyToAsync(stream);
            }

            return nome;
        }

        [Authorize]
        // GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Filme == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }
            ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome", filme.ArtistaId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Nome", filme.GeneroId);
            ViewData["SituacaoId"] = new SelectList(_context.Situacao, "Id", "Status", filme.SituacaoId);
            return View(filme);
        }
        [Authorize]
        // POST: Filmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodigoDeBarras,Titulo,GeneroId,Ano,Tipo,Preco,DataAdquirida,ValorCusto,SituacaoId,ArtistaId,Diretor,Imagem")] Filme filme, IFormFile anexo)
        {
            ModelState.Remove("Artista");
            ModelState.Remove("Genero");
            ModelState.Remove("Situacao");
            ModelState.Remove("Imagem");

            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistaId"] = new SelectList(_context.Artista, "Id", "Nome", filme.ArtistaId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Nome", filme.GeneroId);
            ViewData["SituacaoId"] = new SelectList(_context.Situacao, "Id", "Status", filme.SituacaoId);
            return View(filme);
        }
        [Authorize]
        // GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Filme == null)
            {
                return Problem("Entity set 'ProjetoAnderson2Context.Filme'  is null.");
            }
            var filme = await _context.Filme.FindAsync(id);
            string filePathName = _filePath + "\\fotos\\" + filme.Imagem;
            if (System.IO.File.Exists(filePathName))
                System.IO.File.Delete(filePathName);

            if (filme != null)
            {
                _context.Filme.Remove(filme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return (_context.Filme?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}