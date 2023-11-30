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
    [Authorize]
    public class ArtistasController : Controller
    {
        private readonly LocadoraContext _context;
        private string _filePath;

        public ArtistasController(LocadoraContext context, IWebHostEnvironment env)
        {
            _filePath = env.WebRootPath;
            _context = context;
        }

        // GET: Artistas
        // GET: Movies
        public async Task<IActionResult> Index(string? paisSelecionado, string searchString)
        {
            if (_context.Artista == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            // Use LINQ to get list of genres.
            //IQueryable<string> paisQuery = from m in _context.Artista
            //                               orderby m.PaisDeNascimento
            //                               select m.PaisDeNascimento;

            IQueryable<string> paisQuery = _context.Artista
                .OrderBy(x => x.PaisDeNascimento)
                .Select(x => x.PaisDeNascimento)
                .AsQueryable()!;

            //var artista = from a in _context.Artista
            //              select a;

            IQueryable<Artista> artista = _context.Artista
                .AsQueryable()!;

            if (!string.IsNullOrEmpty(searchString))
            {
                artista = artista.Where(s => s.Nome!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(paisSelecionado))
            {
                artista = artista.Where(x => x.PaisDeNascimento == paisSelecionado);
            }

            var retornandoTela = new PesquisaArtista
            {
                PaisDeNascimento2 = new SelectList(await paisQuery.Distinct().ToListAsync()),
                Artistas = await artista.ToListAsync()
            };

            return View(retornandoTela);
        }

        // GET: Artistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artista == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // GET: Artistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataDeNascimento,PaisDeNascimento,Imagem")] Artista artista, IFormFile anexo)
        {
            ModelState.Remove("Imagem");

            if (ModelState.IsValid)
            {
                if (!ValidaImagem(anexo))
                    return View(artista);

                var nome = SalvarArquivo(anexo);
                artista.Imagem = nome;

                _context.Add(artista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
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

        // GET: Artistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artista == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista.FindAsync(id);
            if (artista == null)
            {
                return NotFound();
            }
            return View(artista);
        }

        // POST: Artistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataDeNascimento,PaisDeNascimento,Imagem")] Artista artista, IFormFile anexo)
        {
            ModelState.Remove("Imagem");
            
            if (id != artista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!ValidaImagem(anexo))
                    return View(artista);

                var nome = SalvarArquivo(anexo);
                artista.Imagem = nome;

                try
                {
                    _context.Update(artista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistaExists(artista.Id))
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
            return View(artista);
        }

        // GET: Artistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artista == null)
            {
                return NotFound();
            }

            var artista = await _context.Artista
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // POST: Artistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artista == null)
            {
                return Problem("Entity set 'ProjetoAnderson2Context.Artista'  is null.");
            }
            var artista = await _context.Artista.FindAsync(id);
            if (artista != null)
            {
                _context.Artista.Remove(artista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistaExists(int id)
        {
            return (_context.Artista?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}