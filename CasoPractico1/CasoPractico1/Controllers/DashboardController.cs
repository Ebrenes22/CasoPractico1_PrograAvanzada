using CasoPractico1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasoPractico1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TransporteDbContext _context;

        public DashboardController(TransporteDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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
