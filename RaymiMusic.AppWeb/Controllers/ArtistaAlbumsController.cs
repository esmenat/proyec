//using System;
//using System.IO;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using RaymiMusic.Api.Data;
//using RaymiMusic.AppWeb.Models;
//using RaymiMusic.Modelos;

//namespace RaymiMusic.AppWeb.Controllers
//{
//    [Authorize(Roles = "artista")]
//    public class ArtistaAlbumsController : Controller
//    {
//        private readonly AppDbContext _ctx;
//        private readonly IWebHostEnvironment _env;

//        public ArtistaAlbumsController(AppDbContext ctx, IWebHostEnvironment env)
//        {
//            _ctx = ctx;
//            _env = env;
//        }

//        private Guid CurrentUserId =>
//            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

//        // Index para mostrar los álbumes
//        public async Task<IActionResult> Index()
//        {
//            var albums = await _ctx.Albumes
//                .Where(a => a.ArtistaId == CurrentUserId)
//                .ToListAsync();
//            return View(albums);
//        }

//        // Detalles de un álbum
//        public async Task<IActionResult> Details(Guid id)
//        {
//            var album = await _ctx.Albumes
//                .Include(a => a.Artista)
//                .Include(a => a.CancionesAlbum) // Cargar canciones asociadas
//                    .ThenInclude(ca => ca.Cancion) // Incluir la canción
//                .FirstOrDefaultAsync(a => a.Id == id && a.ArtistaId == CurrentUserId);

//            if (album == null) return NotFound();

//            return View(album);
//        }

//        // Crear un álbum
//        public async Task<IActionResult> Create()
//        {
//            var canciones = await _ctx.Canciones
//                .Where(c => c.ArtistaId == CurrentUserId)
//                .Select(c => new SelectListItem
//                {
//                    Value = c.Id.ToString(),
//                    Text = c.Titulo
//                })
//                .ToListAsync();

//            var model = new AlbumCreateVM
//            {
//                ArtistaId = CurrentUserId,
//                Canciones = canciones
//            };

//            return View(model);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(AlbumCreateVM vm)
//        {
//            if (!ModelState.IsValid)
//            {
//                vm.Canciones = await _ctx.Canciones
//                    .Where(c => c.ArtistaId == CurrentUserId)
//                    .Select(c => new SelectListItem
//                    {
//                        Value = c.Id.ToString(),
//                        Text = c.Titulo
//                    })
//                    .ToListAsync();
//                return View(vm);
//            }

//            // Definir la variable nombreArchivoPortada
//            string? nombreArchivoPortada = null;

//            // Guardar portada
//            if (vm.Portada != null)
//            {
//                var extension = Path.GetExtension(vm.Portada.FileName).ToLower();
//                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
//                if (!allowedExtensions.Contains(extension))
//                {
//                    ModelState.AddModelError("Portada", "El archivo debe ser una imagen JPG, PNG o GIF.");
//                    return View(vm);
//                }

//                var nombreArchivo = Guid.NewGuid().ToString() + extension;
//                var ruta = Path.Combine(_env.WebRootPath, "media/portadas", nombreArchivo);
//                using var stream = new FileStream(ruta, FileMode.Create);
//                await vm.Portada.CopyToAsync(stream);
//                nombreArchivoPortada = nombreArchivo; // Asignar el nombre del archivo aquí
//            }

//            // Crear el álbum
//            var album = new Album
//            {
//                Id = Guid.NewGuid(),
//                Titulo = vm.Titulo,
//                FechaLanzamiento = vm.FechaLanzamiento ?? DateTime.Now,
//                ArtistaId = CurrentUserId,
//                NombreArchivoPortada = nombreArchivoPortada // Usar la variable aquí
//            };

//            _ctx.Albumes.Add(album);

//            // Relacionar canciones seleccionadas
//            if (vm.CancionesSeleccionadas != null && vm.CancionesSeleccionadas.Any())
//            {
//                foreach (var idCancion in vm.CancionesSeleccionadas)
//                {
//                    _ctx.CancionAlbum.Add(new CancionAlbum
//                    {
//                        AlbumId = album.Id,
//                        CancionId = idCancion
//                    });
//                }
//            }
//            else
//            {
//                ModelState.AddModelError("CancionesSeleccionadas", "Debe seleccionar al menos una canción.");
//                return View(vm);
//            }

//            await _ctx.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        public async Task<IActionResult> Edit(Guid id)
//        {
//            var album = await _ctx.Albumes.FirstOrDefaultAsync(a => a.Id == id && a.ArtistaId == CurrentUserId);
//            if (album == null) return NotFound();

//            return View(new AlbumEditVM
//            {
//                Id = album.Id,
//                Titulo = album.Titulo,
//                FechaLanzamiento = album.FechaLanzamiento,
//                ArtistaId = album.ArtistaId,
//                PortadaActual = album.NombreArchivoPortada
//            });
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(Guid id, AlbumEditVM vm)
//        {
//            if (!ModelState.IsValid) return View(vm);

//            var album = await _ctx.Albumes.FirstOrDefaultAsync(a => a.Id == id && a.ArtistaId == CurrentUserId);
//            if (album == null) return NotFound();

//            album.Titulo = vm.Titulo;
//            album.FechaLanzamiento = vm.FechaLanzamiento;

//            if (vm.Portada != null)
//            {
//                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(vm.Portada.FileName);
//                var ruta = Path.Combine(_env.WebRootPath, "media/portadas", nombreArchivo);
//                using var stream = new FileStream(ruta, FileMode.Create);
//                await vm.Portada.CopyToAsync(stream);
//                album.NombreArchivoPortada = nombreArchivo;
//            }

//            await _ctx.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        // Eliminar una canción del álbum
//        public async Task<IActionResult> RemoveSong(Guid albumId, Guid cancionId)
//        {
//            var album = await _ctx.Albumes
//                .Include(a => a.CancionesAlbum)
//                .FirstOrDefaultAsync(a => a.Id == albumId && a.ArtistaId == CurrentUserId);

//            if (album == null) return NotFound();

//            var cancionAlbum = album.CancionesAlbum.FirstOrDefault(ca => ca.CancionId == cancionId);
//            if (cancionAlbum != null)
//            {
//                _ctx.CancionAlbum.Remove(cancionAlbum);
//                await _ctx.SaveChangesAsync();
//            }

//            return RedirectToAction(nameof(Edit), new { id = albumId });
//        }

//        // Eliminar un álbum
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var album = await _ctx.Albumes
//                .Include(a => a.Artista)
//                .FirstOrDefaultAsync(a => a.Id == id && a.ArtistaId == CurrentUserId);

//            if (album == null) return NotFound();

//            return View(album);
//        }

//        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(Guid id)
//        {
//            var album = await _ctx.Albumes
//                .Include(a => a.CancionesAlbum)
//                .FirstOrDefaultAsync(a => a.Id == id && a.ArtistaId == CurrentUserId);

//            if (album == null) return NotFound();

//            _ctx.CancionAlbum.RemoveRange(album.CancionesAlbum);
//            _ctx.Albumes.Remove(album);

//            await _ctx.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
