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
    public class SituacoesController : Controller
    {
        private readonly LocadoraContext _context;

        public SituacoesController(LocadoraContext context)
        {
            _context = context;
        }

        // GET: Situacoes
        public async Task<IActionResult> Index()
        {
              return _context.Situacao != null ? 
                          View(await _context.Situacao.ToListAsync()) :
                          Problem("Entity set 'ProjetoAnderson2Context.Situacao'  is null.");
        }

        // GET: Situacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Situacao == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (situacao == null)
            {
                return NotFound();
            }

            return View(situacao);
        }

        // GET: Situacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Situacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status")] Situacao situacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(situacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(situacao);
        }

        // GET: Situacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Situacao == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacao.FindAsync(id);
            if (situacao == null)
            {
                return NotFound();
            }
            return View(situacao);
        }

        // POST: Situacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Situacao situacao)
        {
            if (id != situacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(situacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SituacaoExists(situacao.Id))
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
            return View(situacao);
        }

        // GET: Situacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Situacao == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (situacao == null)
            {
                return NotFound();
            }

            return View(situacao);
        }

        // POST: Situacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Situacao == null)
            {
                return Problem("Entity set 'ProjetoAnderson2Context.Situacao'  is null.");
            }
            var situacao = await _context.Situacao.FindAsync(id);
            if (situacao != null)
            {
                _context.Situacao.Remove(situacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SituacaoExists(int id)
        {
          return (_context.Situacao?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
