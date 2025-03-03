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
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "Hora");
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "NombreRuta");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "NombreUsuario");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa");
            return View();
        }

        // POST: Boletos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoletoId,UsuarioId,HorarioId,VehiculoId,RutaId")] Boleto boleto)
        {
            boleto.FechaCompra = DateTime.Now;
            boleto.UsuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;

            ModelState.Remove("Usuario");
            ModelState.Remove("Horario");
            ModelState.Remove("Vehiculo");
            ModelState.Remove("Ruta");

            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.VehiculoId == boleto.VehiculoId);
            if (vehiculo == null)
            {
                ModelState.AddModelError("VehiculoId", "El vehículo seleccionado no existe.");
                return View(boleto);
            }

            int boletosVendidos = await _context.Boletos
                .CountAsync(b => b.HorarioId == boleto.HorarioId && b.VehiculoId == boleto.VehiculoId);

            if (boletosVendidos >= vehiculo.Capacidad)
            {
                ModelState.AddModelError("", "No hay asientos disponibles para este horario y vehículo.");
                return View(boleto);
            }

            if (ModelState.IsValid)
            {
                _context.Add(boleto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "Hora", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "NombreRuta", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "NombreUsuario", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", boleto.VehiculoId);

            return View(boleto);
        }


        // GET: Boletos/Edit/5
        // GET: Boletoes/Edit/5
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
            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "Hora", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "NombreRuta", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "NombreUsuario", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", boleto.VehiculoId);
            return View(boleto);
        }

        // POST: Boletoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoletoId,UsuarioId,HorarioId,VehiculoId,RutaId")] Boleto boleto)
        {
            if (id != boleto.BoletoId)
            {
                return NotFound();
            }

            var boletoExistente = await _context.Boletos.AsNoTracking().FirstOrDefaultAsync(b => b.BoletoId == id);
            if (boletoExistente == null)
            {
                return NotFound();
            }

            // Restaurar el valor de FechaCompra
            boleto.FechaCompra = boletoExistente.FechaCompra;

            ModelState.Remove("Usuario");
            ModelState.Remove("Horario");
            ModelState.Remove("Vehiculo");
            ModelState.Remove("Ruta");

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

            ViewData["HorarioId"] = new SelectList(_context.Horarios, "HorarioId", "Hora", boleto.HorarioId);
            ViewData["RutaId"] = new SelectList(_context.Rutas, "RutaId", "NombreRuta", boleto.RutaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "NombreUsuario", boleto.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "VehiculoId", "Placa", boleto.VehiculoId);

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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BoletoExists(int id)
        {
            return _context.Boletos.Any(e => e.BoletoId == id);
        }

        public async Task<IActionResult> Resumen()
        {
            // rutas activas
            int rutasActivas = await _context.Rutas.CountAsync(r => r.Estado == "Activo");
            Console.WriteLine("total rutas activas: " + rutasActivas);

            // vehiculos en estado bueno
            int vehiculosEnBuenEstado = await _context.Vehiculos.CountAsync(v => v.Estado == "Bueno");
            Console.WriteLine("total vehiculos en buen estado: " + vehiculosEnBuenEstado);

            // numero total de boletos vendidos
            DateTime primerDiaDelMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int boletosVendidosMes = await _context.Boletos
                .CountAsync(b => b.FechaCompra >= primerDiaDelMes);
            Console.WriteLine("total boletos del mes: " + boletosVendidosMes);


            var resumen = new
            {
                RutasActivas = rutasActivas,
                VehiculosEnBuenEstado = vehiculosEnBuenEstado,
                BoletosVendidosMes = boletosVendidosMes
            };

            return View(resumen);
        }

    }
}
