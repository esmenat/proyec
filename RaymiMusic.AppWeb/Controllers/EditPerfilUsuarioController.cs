using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using RaymiMusic.Modelos.ViewModels;

namespace RaymiMusic.AppWeb.Controllers
{
    public class EditPerfilUsuarioController : Controller
    {
        private readonly AppDbContext _ctx;
        public EditPerfilUsuarioController(AppDbContext ctx) => _ctx = ctx;

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return NotFound();

            var usuario = await _ctx.Usuarios
                .Include(u => u.Follows)
                .ThenInclude(f => f.Artista)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

            if (usuario == null)
                return NotFound();

            var vm = new UsuarioPerfilVM
            {
                Id = usuario.Id,
                Correo = usuario.Correo
            };
            ViewBag.FotoUrlUsuario = usuario.UrlFotoPerfil ?? "default-user.png";

            var artistasSeguidos = usuario.Follows
     .Where(f => f.Artista != null)
     .Select(f => new ArtistaSeguidoVM
     {
         Id = f.Artista.Id,
         Nombre = f.Artista.NombreArtistico,
         UrlFotoPerfil = f.Artista.UrlFotoPerfil
     })
     .ToList();

            ViewBag.ArtistasSeguidos = artistasSeguidos;



            return View("Index", vm);
        }


        private string Hash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UsuarioPerfilVM vm, IFormFile? fotoPerfil)
        {
            if (id != vm.Id)
                return BadRequest();

            var existingUser = await _ctx.Usuarios.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View("Index", vm);

            // ✅ Guardar nueva contraseña si se envía
            if (!string.IsNullOrWhiteSpace(vm.HashContrasena))
            {
                existingUser.HashContrasena = Hash(vm.HashContrasena);
            }

            // ✅ Guardar nueva imagen si se envía
            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                var extension = Path.GetExtension(fotoPerfil.FileName);
                var fileName = $"usuario_{existingUser.Id}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/usuarios", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fotoPerfil.CopyToAsync(stream);
                }

                existingUser.UrlFotoPerfil = fileName;
            }

            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
