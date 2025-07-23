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
    public class PagosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPago()
        {
            return await _context.Pago.ToListAsync();
        }

        // GET: api/Pagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(Guid id)
        {
            var pago = await _context.Pago.FindAsync(id);

            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

       
        // PUT: api/Pagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(Guid id, Pago pago)
        {
            if (id != pago.Id)
            {
                return BadRequest();
            }

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
            var usuario = await _context.Usuarios.FindAsync(pago.UsuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }
            if (pago.Monto <= 0)
            {
                return BadRequest("El monto del pago debe ser mayor a cero");
            }
            if (string.IsNullOrEmpty(pago.NumeroDeTarjeta) || pago.NumeroDeTarjeta.Length < 16)
            {
                return BadRequest("Número de tarjeta inválido");
            }
            if (string.IsNullOrEmpty(pago.NombreTitular))
            {
                return BadRequest("El nombre del titular es obligatorio");
            }
            if (pago.FechaExpiracion < DateTime.Now)
            {
                return BadRequest("La fecha de expiración de la tarjeta no puede ser anterior a la fecha actual");
            }

            usuario.PlanSuscripcionId = pago.PlanSuscripcionId;
            _context.Entry(usuario).State = EntityState.Modified;

            _context.Pago.Add(pago);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetPago", new { id = pago.Id }, pago);
        }

        // DELETE: api/Pagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(Guid id)
        {
            var pago = await _context.Pago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            _context.Pago.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(Guid id)
        {
            return _context.Pago.Any(e => e.Id == id);
        }
    }
}
