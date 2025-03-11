using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.HomePage;

namespace OnlineMusicProject.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly OnlineMusicDBContext _context;

        public ArtistsController(OnlineMusicDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SongsOf(string artist)
        {
            if (string.IsNullOrEmpty(artist))
            {
                return NotFound();
            }
            var artists = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistName == artist);
            if (artists == null)
            {
                return NotFound();
            }
            var songs = await _context.Songs.Include(s => s.Artists).Where(s => s.ArtistId == artists.ArtistId).ToListAsync();
            var songsOfartists = new SongArtistViewModel
            {
                ArtistOfSongs = artists,
                Songs = songs
            };
            return View(songsOfartists);
        }
    }
}
