using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.AppWeb.Services;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService _plService;
        public PlaylistsController(IPlaylistService plService) =>
            _plService = plService;

        // GET: /Playlists
        public async Task<IActionResult> Index()
        {
            var listas = await _plService.GetAllAsync();
            return View(listas);
        }
        public async Task<IActionResult> PlaylistPublicas()
        {
            var listas = await _plService.GetPublicPlaylistsAsync();
            return View(listas);
        }
        public async Task<IActionResult> MisPlaylist()
        {
           
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return NotFound();
            var listas = await _plService.GetListasUsuario(Guid.Parse(userId));

            return View(listas);
        }
        // GET: /Playlists/Create
        public IActionResult Create()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var vm = new CreatePlaylistVM
            {
                UsuarioId = userId
            };
            return View(vm);
        }
        // GET: /Playlists/Create
        public IActionResult CreateByUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var vm = new CreatePlaylistVM
            {
                UsuarioId = userId
            };
            return View(vm);
        }

        // POST: /Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlaylistVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _plService.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByUser(CreatePlaylistVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _plService.CreateAsync(vm);
            return RedirectToAction(nameof(MisPlaylist));
        }

        // GET: /Playlists/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _plService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
        // GET: /Playlists/Details/{id}
        public async Task<IActionResult> DetailsByUser(Guid id)
        {
            var vm = await _plService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
        // GET: /Playlists/Details/{id}
        public async Task<IActionResult> DetailsByUserPersonal(Guid id)
        {
            var vm = await _plService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
        // GET: /Playlists/AddSong/3e...-cancionId
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddSong(Guid songId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return NotFound();
            var listas = await _plService.GetListasUsuario(Guid.Parse(userId));
            var vm = new AddSongVM
            {
                SongId = songId,
                Playlists = listas
            };
            return View(vm);
        }

        // POST: /Playlists/AddSong
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddSong(AddSongVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _plService.AddSongAsync(vm.PlaylistId, vm.SongId);

            // Opcional: volver al reproductor o al detalle de la playlist
            return RedirectToAction("Play", "Player", new { id = vm.SongId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemoveSong(AddSongVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            await _plService.RemoveSongAsync(vm.PlaylistId, vm.SongId);
            return RedirectToAction("DetailsByUserPersonal", new { id = vm.PlaylistId });
        }

    }
}
