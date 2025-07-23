using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescargasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DescargasController(AppDbContext context) 
        {
            _context = context;
        }

        // POST: api/Descargas/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarDescarga([FromBody] Descarga descarga)
        {
            if (descarga == null || descarga.CancionId == Guid.Empty)
                return BadRequest("ID de canción inválido.");

            // Verifica si la canción existe antes de guardar
            var existe = await _context.Canciones.AnyAsync(c => c.Id == descarga.CancionId);
            if (!existe)
                return NotFound("Canción no encontrada.");

            var nueva = new Descarga
            {
                CancionId = descarga.CancionId
            };

            _context.Descargas.Add(nueva);
            await _context.SaveChangesAsync();

            // Total de descargas para esa canción (opcional)
            var total = await _context.Descargas.CountAsync(d => d.CancionId == descarga.CancionId);

            return Ok(new { mensaje = "Descarga registrada", totalDescargas = total });
        }

        // GET: api/Descargas/total/{id}
        [HttpGet("total/{id}")]
        public async Task<ActionResult<int>> ObtenerTotalDescargasPorCancion(Guid id)
        {
            var existe = await _context.Canciones.AnyAsync(c => c.Id == id);
            if (!existe)
                return NotFound("Canción no encontrada");

            var total = await _context.Descargas
                .CountAsync(d => d.CancionId == id);

            return Ok(total);
        }

    }
}
