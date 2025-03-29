using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;

namespace OnlineMusicProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly OnlineMusicDBContext _context;

        public SearchController(OnlineMusicDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index", "Home");
            }
            var isArtist = _context.Artists.Where(s => s.ArtistName.Contains(searchQuery));
            if (isArtist.Any())
            {
                return RedirectToAction("Artists", "Users", new { searchQuery });
            }
            var isSongs = _context.Songs.Include(s => s.Artists).Where(s => s.NameSong.Contains(searchQuery));
            if (isSongs.Any()){
                return RedirectToAction("SongList", "Songs", new { searchQuery });
            }
            var isAlbums = _context.Albums.Include(s => s.Artists).Where(al => al.Title.Contains(searchQuery));
            if (isAlbums.Any())
            {
                return RedirectToAction("List", "Albums", new { searchQuery });
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
