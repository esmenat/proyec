// RaymiMusic.AppWeb.Models/AlbumCreateVM.cs
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RaymiMusic.AppWeb.Models
{
    public class AlbumCreateVM
    {
        [Required]
        public string Titulo { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? FechaLanzamiento { get; set; }
        public IFormFile? Portada { get; set; }
        public string? PortadaActual { get; set; }

        [Required]
        public Guid ArtistaId { get; set; }

        // Sólo para el dropdown de Admin
        public SelectList? Artistas { get; set; }

        // Para el rol Artista: lista de canciones disponibles
        public List<SelectListItem>? Canciones { get; set; }
        public List<Guid>? CancionesSeleccionadas { get; set; } = new List<Guid>();

    }

}
