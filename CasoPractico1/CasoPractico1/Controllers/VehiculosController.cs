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
    public class VehiculosController : Controller
    {
        private readonly TransporteDbContext _context;

        public VehiculosController(TransporteDbContext context)
        {
            _context = context;
        }

        // GET: Vehiculos
        public async Task<IActionResult> Index()
        {
            var transporteDbContext = _context.Vehiculos.Include(v => v.Ruta);
            return View(await transporteDbContext.ToListAsync());
        }

        
        // GET: Vehiculos/Create
        public IActionResult Create()
        {
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "NombreRuta");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Placa,Modelo,Capacidad,Estado,RutaId")] Vehiculo vehiculo)
        {

            vehiculo.FechaRegistro = DateTime.Now;
            vehiculo.UsuarioRegistroId = (int)HttpContext.Session.GetInt32("UsuarioId"); 

            ModelState.Remove("Boletos");
            ModelState.Remove("Usuario");
            ModelState.Remove("Ruta");

            if (VehiculoExists(vehiculo.Placa))
            {
                ModelState.AddModelError("Placa", "Ya existe un vehiculo con esa placa");
                return View(vehiculo);
            }

                if (ModelState.IsValid)
            {
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", vehiculo.RutaId);
            return View(vehiculo);
        }

        // GET: Vehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", vehiculo.RutaId);
            ViewData["estado"] = vehiculo.Estado;
            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehiculoId,Placa,Modelo,Capacidad,Estado,RutaId,FechaRegistro,UsuarioRegistroId")] Vehiculo vehiculo)
        {
            if (id != vehiculo.VehiculoId)
            {
                return NotFound();
            }


            ModelState.Remove("Boletos");
            ModelState.Remove("Usuario");
            ModelState.Remove("Ruta");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Placa))
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
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "RutaId", vehiculo.RutaId);
            return View(vehiculo);
        }

        // GET: Vehiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Ruta)
                .FirstOrDefaultAsync(m => m.VehiculoId == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(string id)
        {
            return _context.Vehiculos.Any(e => e.Placa == id);
        }
    }
}
