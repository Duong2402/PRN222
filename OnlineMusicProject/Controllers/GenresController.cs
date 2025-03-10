using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.HomePage;

namespace OnlineMusicProject.Controllers
{
    public class GenresController : Controller
    {
        private readonly OnlineMusicDBContext _context;

        public GenresController(OnlineMusicDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Songs(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return NotFound();
            }
            var genres = await _context.SongGenres.FirstOrDefaultAsync(g => g.Name == genre);
            if (genres == null)
            {
                return NotFound();
            }
            var songs = await _context.Songs.Include(s => s.Artists).Where(s => s.GenreId == genres.GenreId).ToListAsync();
            var songsGenre = new SongsGenresViewModel
            {
                Songs = songs,
                SongGenres = genres,
                
            };
            return View(songsGenre);
        }
    }
}
