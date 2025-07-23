using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.AppWeb.Services;

namespace RaymiMusic.AppWeb.Controllers
{
    public class PlansUsuariosController : Controller
    {
        private readonly IPlanesService _planesService;
        public PlansUsuariosController(IPlanesService planesService)
        {
            _planesService = planesService;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarPlanFree()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            var userId = Guid.Parse(userIdClaim.Value);

            try
            {
                
                await _planesService.AsignarPlan(userId, "Free");
                return RedirectToAction("Planes"); // Redirige nuevamente a la vista de planes
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al asignar el plan: {ex.Message}";
                return RedirectToAction("Planes");
            }
        }

        public async Task<IActionResult> Planes()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return RedirectToAction("Login", "Account");

                var userId = Guid.Parse(userIdClaim.Value);
                var plan = await _planesService.ObtenerPlanUsuarioAsync(userId);

                ViewBag.PlanActual = plan.Nombre;
                return View();
            }
            catch (Exception ex)
            {

                ViewBag.PlanActual = "Desconocido";
                ViewBag.Error = ex.Message;
                return View();
            }
        }


    }
}
