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
    public class UsuariosController : Controller
    {
        private readonly TransporteDbContext _context;

        public UsuariosController(TransporteDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario,NombreCompleto,Correo,Telefono,Contrasena,Rol")] Usuario usuario)
        {

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El usuario se creó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            // Si hay error, se vuelve a mostrar la vista Create
            TempData["ErrorMessage"] = "Ocurrió un error al crear el usuario. Verifica los datos e intenta nuevamente.";
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }



        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,NombreUsuario,NombreCompleto,Correo,Telefono,Contrasena,Rol")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.UsuarioId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruta = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {

            
                _context.Usuarios.Remove(usuario);
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

 