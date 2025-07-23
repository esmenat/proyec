using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaymiMusic.AppWeb.Services;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Controllers
{
    public class PagoController : Controller
    {

        private readonly IPlanesService _planesService;

        public PagoController(IPlanesService planesService)
        {
            _planesService = planesService;
        }
        // GET: PagoController/Create
        public async Task<IActionResult> Create(string plan)
        {
            if (string.IsNullOrEmpty(plan))
                return BadRequest("Debe especificar un plan");

            var planDatos = await _planesService.ObtenerPlanPorNombreAsync(plan);
            if (planDatos == null)
                return NotFound("Plan no encontrado");

            ViewBag.PlanSeleccionado = plan;
            ViewBag.PlanDatos = planDatos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                // Obtener el plan desde el formulario
                var nombrePlan = collection["NombrePlan"];
                var plan = await _planesService.ObtenerPlanPorNombreAsync(nombrePlan);

                if (plan == null)
                    return NotFound("Plan no encontrado");

                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return NotFound();
                }

                // Crear la instancia de Pago
                var pago = new Pago
                {
                    Id = Guid.NewGuid(),
                    UsuarioId =Guid.Parse(userId),
                    FechaPago = DateTime.Now,
                    Monto = plan.Precio,
                    NumeroDeTarjeta = collection["NumeroDeTarjeta"],
                    NombreTitular = collection["NombreTitular"],
                    CodigoSeguridad = collection["CodigoSeguridad"],
                    FechaExpiracion = DateTime.Parse(collection["FechaExpiracion"]),
                    PlanSuscripcionId = plan.Id,
                    Estado = "Procesado"
                };
                await _planesService.RealizarPago(pago);
                return RedirectToAction("Index", "Home");
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al procesar el pago: " + ex.Message);
                return View();
            }
        }

    }
}
