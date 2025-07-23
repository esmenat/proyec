using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Pago
    {
        [Key] public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string NumeroDeTarjeta { get; set; }
        public Guid PlanSuscripcionId { get; set; }
        public string Estado { get; set; }
        public string NombreTitular { get; set; }
        public string CodigoSeguridad { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public PlanSuscripcion? PlanSuscripcion { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
