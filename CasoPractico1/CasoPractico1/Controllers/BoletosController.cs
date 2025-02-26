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
    public class BoletosController : Controller
    {
        private readonly TransporteDbContext _context;

        public BoletosController(TransporteDbContext context)
        {
            _context = context;
        }

        // GET: Boletoes
        public async Task<IActionResult> Index()
        {
            var transporteDbContext = _context.Boletos.Include(b => b.Horario).Include(b => b.Ruta).Include(b => b.Usuario).Include(b => b.Vehiculo);
            return View(await transporteDbContext.ToListAsync());
        }

        // GET: Boletoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos
                .Include(b => b.Horario)
                .Include(b => b.Ruta)
                .Include(b => b.Usuario)
                .Include(b => b.Vehiculo)
                .FirstOrDefaultAsync(m => m.BoletoId == id);
            if (boleto == null)
            {
                return NotFound();
            }

            return View(boleto);
        }

        // GET: Boletoes/Create
        public IActionResult Create()
        {
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "HorarioId");
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "VehiculoId");
            return View();
        }

        // POST: Boletos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoletoId,UsuarioId,HorarioId,VehiculoId,RutaId,FechaCompra")] Boleto boleto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boleto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "HorarioId", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "VehiculoId", boleto.VehiculoId);
            return View(boleto);
        }

        // GET: Boletos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos.FindAsync(id);
            if (boleto == null)
            {
                return NotFound();
            }
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "HorarioId", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "VehiculoId", boleto.VehiculoId);
            return View(boleto);
        }

        // POST: Boletoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoletoId,UsuarioId,HorarioId,VehiculoId,RutaId,FechaCompra")] Boleto boleto)
        {
            if (id != boleto.BoletoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boleto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoletoExists(boleto.BoletoId))
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
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "HorarioId", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "VehiculoId", boleto.VehiculoId);
            return View(boleto);
        }

        // GET: Boletoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleto = await _context.Boletos
                .Include(b => b.Horario)
                .Include(b => b.Ruta)
                .Include(b => b.Usuario)
                .Include(b => b.Vehiculo)
                .FirstOrDefaultAsync(m => m.BoletoId == id);
            if (boleto == null)
            {
                return NotFound();
            }

            return View(boleto);
        }

        // POST: Boletoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boleto = await _context.Boletos.FindAsync(id);
            if (boleto != null)
            {
                _context.Boletos.Remove(boleto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoletoExists(int id)
        {
            return _context.Boletos.Any(e => e.BoletoId == id);
        }
    }
}
