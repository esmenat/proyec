using Microsoft.AspNetCore.Mvc.Rendering;
using RaymiMusic.Modelos;
using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.AppWeb.Models
{
    public class AlbumEditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Titulo { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? FechaLanzamiento { get; set; }
        public IFormFile? Portada { get; set; }
        public string? PortadaActual { get; set; }

        [Required]
        public Guid ArtistaId { get; set; }

        // Sólo para Admin
        public SelectList? Artistas { get; set; }


        // Lista de canciones seleccionadas para el álbum
        public List<Guid>? CancionesSeleccionadas { get; set; } = new List<Guid>();

        // Lista de canciones disponibles para la selección
        public List<SelectListItem>? CancionesDisponibles { get; set; } = new List<SelectListItem>();
    }
}
