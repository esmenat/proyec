using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaymiMusic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialReproduccionesController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public HistorialReproduccionesController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET: api/HistorialReproducciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialReproducciones>>> GetHistorialReproducciones()
        {
            return await _ctx.HistorialReproducciones.ToListAsync();
        }

        // GET: api/HistorialReproducciones/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialReproducciones>> GetHistorialReproduccion(Guid id)
        {
            var historialReproduccion = await _ctx.HistorialReproducciones.FindAsync(id);

            if (historialReproduccion == null)
            {
                return NotFound();
            }

            return historialReproduccion;
        }

        // POST: api/HistorialReproducciones
        [HttpPost]
        public async Task<ActionResult<HistorialReproducciones>> PostHistorialReproduccion(HistorialReproducciones historialReproduccion)
        {
            historialReproduccion.Id = Guid.NewGuid();
            _ctx.HistorialReproducciones.Add(historialReproduccion);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistorialReproduccion), new { id = historialReproduccion.Id }, historialReproduccion);
        }

        // DELETE: api/HistorialReproducciones/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorialReproduccion(Guid id)
        {
            var historialReproduccion = await _ctx.HistorialReproducciones.FindAsync(id);
            if (historialReproduccion == null)
            {
                return NotFound();
            }

            _ctx.HistorialReproducciones.Remove(historialReproduccion);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/HistorialReproducciones/cancion/{cancionId}/total
        [HttpGet("cancion/{cancionId}/total")]
        public async Task<ActionResult<int>> ObtenerTotalReproduccionesPorCancion(Guid cancionId)
        {
            var total = await _ctx.HistorialReproducciones
                .CountAsync(r => r.CancionId == cancionId);

            return Ok(total);
        }
    }
}
