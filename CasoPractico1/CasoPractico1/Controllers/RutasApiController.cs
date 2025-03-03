using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CasoPractico1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasoPractico1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutasApiController : ControllerBase
    {
        private readonly TransporteDbContext _context;

        public RutasApiController(TransporteDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de todas las rutas disponibles.
        /// </summary>
        /// <returns>Lista de rutas con horarios y paradas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ruta>>> GetRutas()
        {
            return await _context.Rutas
                .Include(r => r.Horarios)
                .Include(r => r.Paradas)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los detalles de una ruta específica.
        /// </summary>
        /// <param name="id">ID de la ruta</param>
        /// <returns>Información de la ruta</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ruta>> GetRuta(int id)
        {
            var ruta = await _context.Rutas
                .Include(r => r.Horarios)
                .Include(r => r.Paradas)
                .FirstOrDefaultAsync(r => r.RutaId == id);

            if (ruta == null)
            {
                return NotFound(new { mensaje = "Ruta no encontrada" });
            }

            return ruta;
        }

        /// <summary>
        /// Obtiene los horarios de una ruta específica.
        /// </summary>
        /// <param name="id">ID de la ruta</param>
        /// <returns>Lista de horarios de la ruta</returns>
        [HttpGet("{id}/horarios")]
        public async Task<ActionResult<IEnumerable<Horario>>> GetHorariosByRuta(int id)
        {
            var horarios = await _context.Horarios
                .Where(h => h.RutaId == id)
                .ToListAsync();

            if (horarios == null || horarios.Count == 0)
            {
                return NotFound(new { mensaje = "No hay horarios disponibles para esta ruta" });
            }

            return horarios;
        }
    }
}