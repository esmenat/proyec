using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public interface IPlanesService 
    {
        Task<PlanSuscripcion> ObtenerPlanUsuarioAsync(Guid UserId);
        Task <PlanSuscripcion> ObtenerPlanPorNombreAsync(string nombrePlan);
        Task RealizarPago(Pago pago);
        Task AsignarPlan(Guid userId, string nombrePlan);
    }
}
