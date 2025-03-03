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
    public class RutasController : Controller
    {
        private readonly TransporteDbContext _context;

        public RutasController(TransporteDbContext context)
        {
            _context = context;
        }

        // GET: Rutas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rutas.Include(r => r.Horarios).ToListAsync());
        }

        // GET: Rutas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Rutas
                .FirstOrDefaultAsync(m => m.RutaId == id);
            if (ruta == null)
            {
                return NotFound();
            }

            return View(ruta);
        }

        // GET: Rutas/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoRuta,NombreRuta,Descripcion,Estado")] Ruta ruta, [Bind("Horarios")] List<string> Horarios, [Bind("Paradas")] List<string> Paradas)
        {
            ruta.FechaRegistro = DateTime.Now;
            ruta.UsuarioRegistroId = (int)HttpContext.Session.GetInt32("UsuarioId"); 

            ModelState.Remove("Boletos");
            ModelState.Remove("Paradas");
            ModelState.Remove("Horarios");
            ModelState.Remove("Vehiculos");
            ModelState.Remove("Usuario");

            if (RutaExists(ruta.CodigoRuta))
            {
                ModelState.AddModelError("CodigoRuta", "El código de ruta ya existe");
                return View(ruta);
            }


            if (ModelState.IsValid)
            {
                _context.Add(ruta);
                await _context.SaveChangesAsync();
                

                var horarios = _context.Horarios.Where(h => h.RutaId == ruta.RutaId);
                _context.Horarios.RemoveRange(horarios);
                await _context.SaveChangesAsync();

                foreach(string horario in Horarios)
                {
                    Horario horarioObj = new Horario();
                    horarioObj.RutaId = ruta.RutaId;
                    horarioObj.Hora = TimeSpan.Parse(horario); 
                    _context.Horarios.Add(horarioObj);
                    await _context.SaveChangesAsync();
                }


                var paradas = _context.Paradas.Where(h => h.RutaId == ruta.RutaId);
                _context.Paradas.RemoveRange(paradas);
                await _context.SaveChangesAsync();

                foreach (string parada in Paradas)
                {
                    Parada paradaObj = new Parada();
                    paradaObj.RutaId = ruta.RutaId;
                    paradaObj.NombreParada = parada;
                    _context.Paradas.Add(paradaObj);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));

            }
            return View(ruta);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta =  _context.Rutas.Include(r => r.Horarios).Include(r => r.Paradas).Where(r => r.RutaId == id).Single();
            if (ruta == null)
            {
                return NotFound();
            }

            ViewBag.ParadasSeleccionadas = ruta.Paradas.Select(p => p.NombreParada).ToList();
            ViewBag.HorariosSeleccionados = ruta.Horarios.Select(h => new DateTime().Add(h.Hora).ToString("HH:mm:ss")).ToList();

            return View(ruta);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RutaId,CodigoRuta,NombreRuta,Descripcion,Estado,FechaRegistro,UsuarioRegistroId")] Ruta ruta, [Bind("Horarios")] List<string> Horarios, [Bind("Paradas")] List<string> Paradas)
        {
            if (id != ruta.RutaId)
            {
                return NotFound();
            }


            ModelState.Remove("Boletos");
            ModelState.Remove("Paradas");
            ModelState.Remove("Horarios");
            ModelState.Remove("Vehiculos");
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruta);
                    await _context.SaveChangesAsync();

                    var horarios = _context.Horarios.Where(h => h.RutaId == ruta.RutaId);
                    _context.Horarios.RemoveRange(horarios);
                    await _context.SaveChangesAsync();

                    foreach (string horario in Horarios)
                    {
                        Horario horarioObj = new Horario();
                        horarioObj.RutaId = ruta.RutaId;
                        horarioObj.Hora = TimeSpan.Parse(horario);
                        _context.Horarios.Add(horarioObj);
                        await _context.SaveChangesAsync();
                    }


                    var paradas = _context.Paradas.Where(h => h.RutaId == ruta.RutaId);
                    _context.Paradas.RemoveRange(paradas);
                    await _context.SaveChangesAsync();

                    foreach (string parada in Paradas)
                    {
                        Parada paradaObj = new Parada();
                        paradaObj.RutaId = ruta.RutaId;
                        paradaObj.NombreParada = parada;
                        _context.Paradas.Add(paradaObj);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RutaExists(ruta.CodigoRuta))
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
            return View(ruta);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Rutas
                .FirstOrDefaultAsync(m => m.RutaId == id);
            if (ruta == null)
            {
                return NotFound();
            }

            return View(ruta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta != null)
            {

                var horarios = _context.Horarios.Where(h => h.RutaId == ruta.RutaId);
                _context.Horarios.RemoveRange(horarios);
                await _context.SaveChangesAsync();

                var paradas = _context.Paradas.Where(h => h.RutaId == ruta.RutaId);
                _context.Paradas.RemoveRange(paradas);
                await _context.SaveChangesAsync();
                _context.Rutas.Remove(ruta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RutaExists(string id)
        {
            return _context.Rutas.Any(e => e.CodigoRuta == id);
        }
    }
}
