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
            List<Songs> songsWeekTop = await _context.Songs
                .Include(s => s.Artists)
                .OrderByDescending(s => s.NumberOfListeners)  
                .Take(5).ToListAsync();

            List<Songs>  songNewHit = await _context.Songs
               .Include(s => s.Artists)
               .Skip(Math.Max(0, _context.Songs.Count() - 5))
               .Take(5).OrderByDescending(s => s.SongId).ToListAsync();

            List<Artists> artists = await _context.Artists
                .OrderByDescending(a => a.ArtistId)
                .Take(6)
                .ToListAsync();

            List<SongGenres> genres = await _context.SongGenres.ToListAsync();
            var song = await _context.Songs.Include(s => s.Artists).OrderByDescending(s => s.NumberOfListeners).FirstOrDefaultAsync();
            var albums = await _context.Albums.Include(a => a.Artists).OrderByDescending(a => a.AlbumId).Take(10).ToListAsync();
            var viewALl = new SongArtistViewModel
            {
                SongsWeekTop = songsWeekTop,
                SongsNewHit = songNewHit,
                Artists = artists,
                Genres = genres,
                MaxListener = song,
                Albums = albums,
                
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
