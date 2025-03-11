using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;

namespace OnlineMusicProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly OnlineMusicDBContext _context;

        public AdminController(UserManager<Users> userManager, OnlineMusicDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Dashboard
        public IActionResult DashBoard() => View();

        // Manage Accounts
        public async Task<IActionResult> ManageAccounts() => View(await _userManager.Users.ToListAsync());


        public IActionResult CreateAccount() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAccount(Users user, string password, string confirmPassword, string role)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
            }

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, role);
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return View(user);
                        }
                    }
                    return RedirectToAction("ManageAccounts");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(Users user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null) return NotFound();
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded) return RedirectToAction("ManageAccounts");
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction("ManageAccounts");
        }

        // Manage Songs
        public async Task<IActionResult> ManageSongs() => View(await _context.Songs.Include(s => s.Artists).Include(s => s.songGenres).ToListAsync());

        public IActionResult CreateSong()
        {
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> EditSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> EditSong(Songs song, IFormFile imageFile, IFormFile audioFile)
        {
            if (ModelState.IsValid)
            {
                var existingSong = await _context.Songs.FindAsync(song.SongId);
                if (existingSong == null) return NotFound();

                existingSong.NameSong = song.NameSong;
                existingSong.ArtistId = song.ArtistId;
                existingSong.GenreId = song.GenreId;
                existingSong.Lyrics = song.Lyrics;

                if (imageFile != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images", imageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    existingSong.ImageSong = "/uploads/images/" + imageFile.FileName;
                }

                if (audioFile != null)
                {
                    var audioPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/audio", audioFile.FileName);
                    using (var stream = new FileStream(audioPath, FileMode.Create))
                    {
                        await audioFile.CopyToAsync(stream);
                    }
                    existingSong.FilePath = "/uploads/audio/" + audioFile.FileName;
                }

                _context.Songs.Update(existingSong);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageSongs");
            }
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(song);
        }

        public async Task<IActionResult> DeleteSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        [HttpPost, ActionName("DeleteSong")]
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
        public async Task<IActionResult> ManagePlaylists() => View(await _context.Playlists.Include(p => p.User).ToListAsync());

        public IActionResult CreatePlaylist()
        {
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(Playlists playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.PlaylistId = Guid.NewGuid();
                _context.Playlists.Add(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePlaylists");
            }
            ViewBag.Users = _userManager.Users.ToList();
            return View(playlist);
        }

        public async Task<IActionResult> EditPlaylist(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) return NotFound();
            ViewBag.Users = _userManager.Users.ToList();
            return View(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlaylist(Playlists playlist)
        {
            if (ModelState.IsValid)
            {
                _context.Playlists.Update(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePlaylists");
            }
            ViewBag.Users = _userManager.Users.ToList();
            return View(playlist);
        }

        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) return NotFound();
            return View(playlist);
        }

        [HttpPost, ActionName("DeletePlaylist")]
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