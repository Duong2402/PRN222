using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels;

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

        // Existing Actions (unchanged)
        [Authorize(Roles = "User, Admin")]
        public IActionResult Profile() => View();

        [HttpPost]
        public async Task<IActionResult> Profile(Users u)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.FullName = u.FullName;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded) return RedirectToAction("Profile");
                    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(u);
        }

        public IActionResult AlbumsStore() => View();

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
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var songHistory = await _context.Histories
                                       .FirstOrDefaultAsync(h => h.UserId == user.Id.ToString()
                                                              && h.SongId == songId);
            if (songHistory == null)
            {
                return RedirectToAction("HistoryOfListening");
            }
            _context.Histories.Remove(songHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("HistoryOfListening");
        }
        public IActionResult Contact() => View();
        public IActionResult Elements() => View();
        public async Task<ActionResult<IEnumerable<Artists>>> Artists() => View(await _context.Artists.ToListAsync());

        // Admin Dashboard Actions
        [Authorize(Roles = "Admin")]
        public IActionResult DashBoard() => View();

        // Manage Accounts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageAccounts() => View(await userManager.Users.ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult CreateAccount() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount(Users user, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded) return RedirectToAction("ManageAccounts");
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(Users user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByIdAsync(user.Id);
                if (existingUser == null) return NotFound();
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                var result = await userManager.UpdateAsync(existingUser);
                if (result.Succeeded) return RedirectToAction("ManageAccounts");
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("DeleteAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccountConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null) await userManager.DeleteAsync(user);
            return RedirectToAction("ManageAccounts");
        }

        // Manage Songs
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageSongs() => View(await _context.Songs.Include(s => s.Artists).ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult CreateSong()
        {
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSong(Songs song)
        {
            if (ModelState.IsValid)
            {
                song.SongId = Guid.NewGuid();
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageSongs");
            }
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(song);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(song);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditSong(Songs song)
        {
            if (ModelState.IsValid)
            {
                _context.Songs.Update(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageSongs");
            }
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(song);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        [HttpPost, ActionName("DeleteSong")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSongConfirmed(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageSongs");
        }

        // Manage Playlists
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManagePlaylists() => View(await _context.Playlists.Include(p => p.User).ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult CreatePlaylist()
        {
            ViewBag.Users = userManager.Users.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlaylist(Playlists playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.PlaylistId = Guid.NewGuid();
                _context.Playlists.Add(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePlaylists");
            }
            ViewBag.Users = userManager.Users.ToList();
            return View(playlist);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPlaylist(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) return NotFound();
            ViewBag.Users = userManager.Users.ToList();
            return View(playlist);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPlaylist(Playlists playlist)
        {
            if (ModelState.IsValid)
            {
                _context.Playlists.Update(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePlaylists");
            }
            ViewBag.Users = userManager.Users.ToList();
            return View(playlist);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) return NotFound();
            return View(playlist);
        }

        [HttpPost, ActionName("DeletePlaylist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlaylistConfirmed(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManagePlaylists");
        }
    }
}