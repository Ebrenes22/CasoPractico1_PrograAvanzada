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

        public  UsuariosController(TransporteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
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

        public IActionResult Details()
        {

            return View();
        }


    }
}
