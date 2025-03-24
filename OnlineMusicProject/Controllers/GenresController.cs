using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.Services.Pagination;
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
        public async Task<IActionResult> Songs(string genre, int page = 1)
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
            int pageSize = 12;
            var songslist = await _context.Songs.Include(s => s.Artists).Where(s => s.GenreId == genres.GenreId).ToListAsync();
            var pageResult = PageResult.ToPaginatedList(songslist, page, pageSize);
            var songsGenre = new SongsGenresViewModel
            {
                Songs = pageResult,
                SongGenres = genres,

            };
            return View(songsGenre);
        }
    }
}
