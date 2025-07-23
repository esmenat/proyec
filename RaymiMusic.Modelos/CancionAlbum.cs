using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class CancionAlbum
    {
        [Key]public Guid id { get; set; }
        public Guid CancionId { get; set; }
        public Cancion Cancion { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
