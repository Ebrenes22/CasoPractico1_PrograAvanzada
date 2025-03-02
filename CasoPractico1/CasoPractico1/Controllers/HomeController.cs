using Microsoft.AspNetCore.Mvc;
using CasoPractico1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace CasoPractico1.Controllers
{
    public class HomeController : Controller
    {
        private readonly TransporteDbContext _context;

        public HomeController(TransporteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("NombreUsuario") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }


        // GET: Home/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Usuario model)
        {
           
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == model.NombreUsuario && u.Contrasena == model.Contrasena);

            if (usuario == null)
            {
                ViewData["ErrorMessage"] = "Nombre de usuario o contraseña incorrectos";
                return View(model);
            }


            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("NombreCompleto", usuario.NombreCompleto);
            HttpContext.Session.SetString("Rol", usuario.Rol);
            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);

            return RedirectToAction("Index", "Home");
        }


        // Cerrar sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
