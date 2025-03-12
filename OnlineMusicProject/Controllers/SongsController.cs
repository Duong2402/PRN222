using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;

namespace OnlineMusicProject.Controllers
{
    public class SongsController : Controller
    {
        private readonly OnlineMusicDBContext _context;

        public SongsController(OnlineMusicDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Songs song = await _context.Songs.Include(p => p.Artists)
                                       .FirstOrDefaultAsync(p => p.SongId == id);
            if(song != null)
            {
                song.NumberOfListeners += 1;
                _context.Songs.Update(song);
                await _context.SaveChangesAsync();
            }
            return View(song);
        }
        public async Task<IActionResult> SongList()
        {
            var songs = await _context.Songs.Include(s => s.Artists).ToListAsync();
            return View(songs);
        }
    }
}
