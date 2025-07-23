using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Follow
    {
        [Key] public int Id { get; set; }
        [Required] public Guid UsuarioId { get; set; } // FK al usuario que sigue
        [Required] public Guid ArtistaId { get; set; } // FK al artista seguido
        [Required] public DateTime FechaSeguimiento { get; set; } = DateTime.UtcNow;
        // Navegación al usuario
        public Usuario? Usuario { get; set; }
        // Navegación al artista
        public Artista? Artista { get; set; }
    }
}