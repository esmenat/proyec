using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Controllers
{
    public class PerfilArtistaController : Controller
    {
        private readonly AppDbContext _ctx;
        public PerfilArtistaController(AppDbContext ctx) => _ctx = ctx;


        // GET: /PerfilArtista
        [HttpGet("PerfilArtista")]
        public async Task<IActionResult> Index()
        {
            var artista = await _ctx.Artistas.FirstOrDefaultAsync(); 

            if (artista == null)
                return NotFound("Artista no encontrado.");

            return View(artista);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var artista = await _ctx.Artistas.FindAsync(id);
            if (artista == null) return NotFound();

            return View(artista);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Artista artista)
        {
            if (!ModelState.IsValid)
                return View(artista);

            var artistaDb = await _ctx.Artistas.FindAsync(artista.Id);
            if (artistaDb == null) return NotFound();

            artistaDb.NombreArtistico = artista.NombreArtistico;
            artistaDb.Biografia = artista.Biografia;
            artistaDb.UrlFotoPerfil = artista.UrlFotoPerfil;
            artistaDb.UrlFotoPortada = artista.UrlFotoPortada;

            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index", "PerfilArtista");
        }
    }
}
