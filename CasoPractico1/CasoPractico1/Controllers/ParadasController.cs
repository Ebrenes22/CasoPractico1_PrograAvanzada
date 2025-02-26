using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasoPractico1.Models;

namespace CasoPractico1.Controllers
{
    public class ParadasController : Controller
    {
        private readonly TransporteDbContext _context;

        public ParadasController(TransporteDbContext context)
        {
            _context = context;
        }

        // GET: Paradas
        public async Task<IActionResult> Index()
        {
            var transporteDbContext = _context.Paradas.Include(p => p.Ruta);
            return View(await transporteDbContext.ToListAsync());
        }

        // GET: Paradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parada = await _context.Paradas
                .Include(p => p.Ruta)
                .FirstOrDefaultAsync(m => m.ParadaId == id);
            if (parada == null)
            {
                return NotFound();
            }

            return View(parada);
        }

        // GET: Paradas/Create
        public IActionResult Create()
        {
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId");
            return View();
        }

        // POST: Paradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParadaId,RutaId,NombreParada")] Parada parada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", parada.RutaId);
            return View(parada);
        }

        // GET: Paradas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parada = await _context.Paradas.FindAsync(id);
            if (parada == null)
            {
                return NotFound();
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", parada.RutaId);
            return View(parada);
        }

        // POST: Paradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParadaId,RutaId,NombreParada")] Parada parada)
        {
            if (id != parada.ParadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParadaExists(parada.ParadaId))
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
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", parada.RutaId);
            return View(parada);
        }

        // GET: Paradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parada = await _context.Paradas
                .Include(p => p.Ruta)
                .FirstOrDefaultAsync(m => m.ParadaId == id);
            if (parada == null)
            {
                return NotFound();
            }

            return View(parada);
        }

        // POST: Paradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parada = await _context.Paradas.FindAsync(id);
            if (parada != null)
            {
                _context.Paradas.Remove(parada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParadaExists(int id)
        {
            return _context.Paradas.Any(e => e.ParadaId == id);
        }
    }
}
