using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.HomePage;

namespace OnlineMusicProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly OnlineMusicDBContext _context;

        public HomeController(OnlineMusicDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Songs> songs = await _context.Songs.Include(s => s.Artists).ToListAsync();
            List<Artists> artists = await _context.Artists.ToListAsync();
            List<SongGenres> genres = await _context.SongGenres.ToListAsync();
            var song = await _context.Songs.Include(s => s.Artists).OrderByDescending(s => s.NumberOfListeners).FirstOrDefaultAsync();

            var viewALl = new SongArtistViewModel
            {
                Songs = songs,
                Artists = artists,
                Genres = genres,
                MaxListener = song,
                
            };
            return View(viewALl);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Errors()
        {
            return View();
        }
    }
}
