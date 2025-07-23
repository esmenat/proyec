using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Descarga
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CancionId { get; set; }
        public Cancion? Cancion { get; set; }

    }
}
