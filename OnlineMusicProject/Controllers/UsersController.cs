using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels;
using OnlineMusicProject.ViewModels.HomePage;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineMusicProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly OnlineMusicDBContext _context;

        public UsersController(UserManager<Users> userManager, OnlineMusicDBContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "User, Admin")]
        public IActionResult Profile() => View();

        [HttpPost]
        public async Task<IActionResult> Profile(Users u, IFormFile avatar)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.FullName = u.FullName;
                    user.PhoneNumber = u.PhoneNumber;

                    if(avatar != null && avatar.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatar-img", avatar.FileName);
                        using(var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await avatar.CopyToAsync(stream);
                        }
                        user.Avatar = "/img/avatar-img/" + avatar.FileName;
                    }
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded) return RedirectToAction("Profile");
                    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(u);
        }

        public IActionResult AlbumsStore() => View();

        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> HistoryOfListening()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var histories = await _context.Histories
                                      .Include(h => h.Songs).ThenInclude(h => h.Artists)
                                      .Where(h => h.UserId == user.Id.ToString())
                                      .ToListAsync();
                var model = new UserProfileViewModel
                {
                    User = user,
                    Histories = histories
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> HistoryOfListening(Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Details", "Songs", new { id = songId });
            var song = await _context.Songs.FindAsync(songId);
            if (song == null)
            {
                return NotFound();
            }
            var history = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == user.Id && h.SongId == songId);

            if (history == null)
            {
                Histories newHistory = new Histories
                {
                    HistoryId = Guid.NewGuid(),
                    UserId = user.Id,
                    SongId = songId,
                    PlayedAt = DateTime.Now,
                };
                _context.Histories.Add(newHistory);
            }
            else
            {
                history.PlayedAt = DateTime.Now;
                _context.Histories.Update(history);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Songs", new { id = songId });
        }

        public async Task<IActionResult> RemoveFromHistories(Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            var songHistory = await _context.Histories
                                       .FirstOrDefaultAsync(h => h.UserId == user.Id.ToString() && h.SongId == songId);
            if (songHistory == null) return RedirectToAction("HistoryOfListening");
            _context.Histories.Remove(songHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("HistoryOfListening");
        }

        public IActionResult Contact() => View();

        public IActionResult Elements() => View();

        public async Task<ActionResult<IEnumerable<Artists>>> Artists() => View(await _context.Artists.ToListAsync());
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Playlist()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var singlePlaylist = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id);
                var play = await _context.Playlists.Where(p => p.UserId == user.Id).ToListAsync();
                var modelPlaylist = new PlaylistsViewModel
                {
                    SinglePlaylist = singlePlaylist,
                    PlaylistList = play
                };
                return View(modelPlaylist);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Playlist(PlaylistsViewModel p, IFormFile avatar)
        {
            var user = await userManager.GetUserAsync(User);
           
            if (user != null)
            {
                Playlists play = new Playlists
                {
                    PlaylistId = Guid.NewGuid(),
                    PlaylistName = p.SinglePlaylist.PlaylistName,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    
                };
                if (avatar != null && avatar.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "playlist-img", avatar.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }

                    play.PlaylistImage = "/img/playlist-img/" + avatar.FileName;
                }
                _context.Playlists.Add(play);
                await _context.SaveChangesAsync();
                return RedirectToAction("PlayList");
            }
            return RedirectToAction("PlayList");
        }

    }
}