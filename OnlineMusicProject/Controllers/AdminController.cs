using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.AdminPage;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IActionResult> Dashboard()
        {
            // Fetch listener data for the last 7 days (simplified since we don't have DateAdded)
            var listenerData = await _context.Songs
                .GroupBy(s => s.SongId) // Group by SongId as a placeholder
                .Select(g => new
                {
                    SongId = g.Key,
                    TotalListeners = g.Sum(s => s.NumberOfListeners)
                })
                .OrderByDescending(g => g.SongId)
                .Take(7) // Take the top 7 songs
                .ToListAsync();

            var labels = listenerData.Select(s => s.SongId.ToString().Substring(0, 8)).ToList(); // Use first 8 chars of Guid for labels
            var data = listenerData.Select(s => s.TotalListeners).ToList();

            ViewData["ChartLabels"] = labels;
            ViewData["ChartData"] = data;

            return View();
        }

        // Manage Accounts
        public async Task<IActionResult> ManageAccounts(int page = 1)
        {
            const int pageSize = 8;
            var accounts = _userManager.Users.AsQueryable();
            var paginatedAccounts = await PaginatedList<Users>.CreateAsync(accounts, page, pageSize);
            return View(paginatedAccounts);
        }

        public IActionResult CreateAccount() => View(new CreateAccountViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            var user = new Users
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }
                return RedirectToAction("ManageAccounts");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EditAccountViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ManageAccounts");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // POST: Handles deletion directly from ManageAccounts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("ManageAccounts");
            }

            // Optional: Prevent deletion of the last admin
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count == 1 && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Error"] = "Cannot delete the last admin account.";
                return RedirectToAction("ManageAccounts");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Account deleted successfully.";
                return RedirectToAction("ManageAccounts");
            }

            TempData["Error"] = "Failed to delete account: " + string.Join(", ", result.Errors.Select(e => e.Description));
            return RedirectToAction("ManageAccounts");
        }
        // Manage Songs
        public async Task<IActionResult> ManageSongs(int page = 1)
        {
            const int pageSize = 8;
            var songs = _context.Songs
                .Include(s => s.Artists)
                .Include(s => s.songGenres)
                .AsQueryable();
            var paginatedSongs = await PaginatedList<Songs>.CreateAsync(songs, page, pageSize);
            return View(paginatedSongs);
        }

        // Create Song (GET)
        public IActionResult CreateSong()
        {
            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(new CreateSongViewModel());
        }

        // Create Song (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSong(CreateSongViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Artists = _context.Artists.ToList();
                ViewBag.Genres = _context.SongGenres.ToList();
                return View(model);
            }

            // Ensure upload directories exist
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var audioDir = Path.Combine(uploadsDir, "audio");
            var imagesDir = Path.Combine(uploadsDir, "images");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }
            if (!Directory.Exists(audioDir))
            {
                Directory.CreateDirectory(audioDir);
            }
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            // Map view model to entity
            var song = new Songs
            {
                SongId = Guid.NewGuid(),
                NameSong = model.NameSong,
                ArtistId = model.ArtistId,
                GenreId = model.GenreId,
                Lyrics = model.Lyrics,
                NumberOfListeners = 0 // Default value
            };

            // Handle audio file (required)
            try
            {
                var audioFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.AudioFile.FileName)}";
                var audioPath = Path.Combine(audioDir, audioFileName);
                using (var stream = new FileStream(audioPath, FileMode.Create))
                {
                    await model.AudioFile.CopyToAsync(stream);
                }
                song.FilePath = "/uploads/audio/" + audioFileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AudioFile", $"Failed to upload audio file: {ex.Message}");
                ViewBag.Artists = _context.Artists.ToList();
                ViewBag.Genres = _context.SongGenres.ToList();
                return View(model);
            }

            // Handle image file (optional)
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var imageFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ImageFile.FileName)}";
                var imagePath = Path.Combine(imagesDir, imageFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                song.ImageSong = "/uploads/images/" + imageFileName;
            }

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Song created successfully.";
            return RedirectToAction("ManageSongs");
        }

        // Edit Song (GET)
        public async Task<IActionResult> EditSong(Guid id)
        {
            var song = await _context.Songs
                .Include(s => s.Artists)
                .Include(s => s.songGenres)
                .FirstOrDefaultAsync(s => s.SongId == id);
            if (song == null) return NotFound();

            var model = new EditSongViewModel
            {
                SongId = song.SongId,
                NameSong = song.NameSong,
                ArtistId = song.ArtistId,
                GenreId = song.GenreId,
                Lyrics = song.Lyrics,
                ImageSong = song.ImageSong,
                FilePath = song.FilePath
            };

            ViewBag.Artists = _context.Artists.ToList();
            ViewBag.Genres = _context.SongGenres.ToList();
            return View(model);
        }

        // Edit Song (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSong(EditSongViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Artists = _context.Artists.ToList();
                ViewBag.Genres = _context.SongGenres.ToList();
                return View(model);
            }

            // Ensure upload directories exist
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var audioDir = Path.Combine(uploadsDir, "audio");
            var imagesDir = Path.Combine(uploadsDir, "images");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }
            if (!Directory.Exists(audioDir))
            {
                Directory.CreateDirectory(audioDir);
            }
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            var song = await _context.Songs.FindAsync(model.SongId);
            if (song == null) return NotFound();

            // Update song properties
            song.NameSong = model.NameSong;
            song.ArtistId = model.ArtistId;
            song.GenreId = model.GenreId;
            song.Lyrics = model.Lyrics;

            // Handle image file (optional)
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var imageFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ImageFile.FileName)}";
                var imagePath = Path.Combine(imagesDir, imageFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                song.ImageSong = "/uploads/images/" + imageFileName;
            }

            // Handle audio file (optional)
            if (model.AudioFile != null && model.AudioFile.Length > 0)
            {
                var audioFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.AudioFile.FileName)}";
                var audioPath = Path.Combine(audioDir, audioFileName);
                using (var stream = new FileStream(audioPath, FileMode.Create))
                {
                    await model.AudioFile.CopyToAsync(stream);
                }
                song.FilePath = "/uploads/audio/" + audioFileName;
            }

            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Song updated successfully.";
            return RedirectToAction("ManageSongs");
        }

        // Delete Song (POST only, directly from ManageSongs)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                TempData["Error"] = "Song not found.";
                return RedirectToAction("ManageSongs");
            }

            try
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Song deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to delete song: {ex.Message}";
            }

            return RedirectToAction("ManageSongs");
        }

        // Manage Playlists
        public async Task<IActionResult> ManagePlaylists(int page = 1)
        {
            const int pageSize = 8;
            var playlists = _context.Playlists
                .Include(p => p.User)
                .AsQueryable();
            var paginatedPlaylists = await PaginatedList<Playlists>.CreateAsync(playlists, page, pageSize);
            return View(paginatedPlaylists);
        }

        // Create Playlist (GET)
        public async Task<IActionResult> CreatePlaylist()
        {
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(new CreatePlaylistViewModel());
        }

        // Create Playlist (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _userManager.Users.ToListAsync();
                return View(model);
            }

            // Ensure upload directories exist
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var imagesDir = Path.Combine(uploadsDir, "images");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            var playlist = new Playlists
            {
                PlaylistId = Guid.NewGuid(),
                PlaylistName = model.PlaylistName,
                UserId = model.UserId,
                CreatedAt = DateTime.UtcNow
            };

            // Handle image file (optional)
            if (model.PlaylistImage != null && model.PlaylistImage.Length > 0)
            {
                var imageFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.PlaylistImage.FileName)}";
                var imagePath = Path.Combine(imagesDir, imageFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.PlaylistImage.CopyToAsync(stream);
                }
                playlist.PlaylistImage = "/uploads/images/" + imageFileName;
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Playlist created successfully.";
            return RedirectToAction("ManagePlaylists");
        }

        // Edit Playlist (GET)
        public async Task<IActionResult> EditPlaylist(Guid id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PlaylistId == id);
            if (playlist == null) return NotFound();

            var model = new EditPlaylistViewModel
            {
                PlaylistId = playlist.PlaylistId,
                PlaylistName = playlist.PlaylistName,
                UserId = playlist.UserId,
                PlaylistImage = playlist.PlaylistImage
            };

            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(model);
        }

        // Edit Playlist (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlaylist(EditPlaylistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _userManager.Users.ToListAsync();
                return View(model);
            }

            // Ensure upload directories exist
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var imagesDir = Path.Combine(uploadsDir, "images");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            var playlist = await _context.Playlists.FindAsync(model.PlaylistId);
            if (playlist == null)
            {
                TempData["Error"] = "Playlist not found.";
                return RedirectToAction("ManagePlaylists");
            }

            // Update playlist properties
            playlist.PlaylistName = model.PlaylistName;
            playlist.UserId = model.UserId;

            // Handle image file (optional)
            if (model.PlaylistImageFile != null && model.PlaylistImageFile.Length > 0)
            {
                var imageFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.PlaylistImageFile.FileName)}";
                var imagePath = Path.Combine(imagesDir, imageFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.PlaylistImageFile.CopyToAsync(stream);
                }
                playlist.PlaylistImage = "/uploads/images/" + imageFileName;
            }

            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Playlist updated successfully.";
            return RedirectToAction("ManagePlaylists");
        }

        // Delete Playlist (POST only, directly from ManagePlaylists)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                TempData["Error"] = "Playlist not found.";
                return RedirectToAction("ManagePlaylists");
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Playlist deleted successfully.";
            return RedirectToAction("ManagePlaylists");
        }
    }
}