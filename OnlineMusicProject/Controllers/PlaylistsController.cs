using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.PlaylistPage;

namespace OnlineMusicProject.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly OnlineMusicDBContext _context;
        private readonly UserManager<Users> userManager;

        public PlaylistsController(OnlineMusicDBContext context, UserManager<Users> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(Guid playlistId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var play = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id && p.PlaylistId == playlistId);
            if(play != null)
            {
                var songsInPlaylist = await _context.PlaylistSongs
                    .Include(ps => ps.Songs)
                    .ThenInclude(ps => ps.Artists)
                    .Where(ps => ps.PlaylistId == play.PlaylistId)
                    .ToListAsync();

                var singleSong = await _context.PlaylistSongs
                    .Include(ps => ps.Songs)
                    .ThenInclude(song => song.Artists)
                    .FirstOrDefaultAsync(ps => ps.PlaylistId == play.PlaylistId);
                var model = new SongsOfPlaylistViewModel
                {
                    playlistItem = play,
                    SinglePlaylistSongs = singleSong,
                    playlistSongs = songsInPlaylist
                };
                return View(model);
            }
            return RedirectToAction("Playlist", "Users");
        }

        public async Task<IActionResult> Delete(Guid playlistId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var play = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id && p.PlaylistId == playlistId);
            if (play == null)
            {
                return RedirectToAction("Playlist", "Users");
            }
            _context.Playlists.Remove(play);
            await _context.SaveChangesAsync();
            return RedirectToAction("Playlist", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid playlistId, string? playlistName, IFormFile playlistImage)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var play = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id && p.PlaylistId == playlistId);
            if (play == null)
            {
                return RedirectToAction("Playlist", "Users");
            }
            if (string.IsNullOrEmpty(playlistName))
            {
                TempData["RequiredMsg"] = "Name is required";
                return RedirectToAction("Details", new { playlistId });
            }
            play.PlaylistName = playlistName;
            if (playlistImage != null && playlistImage.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "playlist-img", playlistImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await playlistImage.CopyToAsync(stream);
                }
                play.PlaylistImage = "/img/playlist-img/" + playlistImage.FileName;
            }
            _context.Playlists.Update(play);
            await _context.SaveChangesAsync();
            return RedirectToAction("Playlist", "Users");
        }
    }
}
