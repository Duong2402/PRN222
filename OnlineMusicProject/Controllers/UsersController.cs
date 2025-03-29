using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.Services.Pagination;
using OnlineMusicProject.ViewModels;
using OnlineMusicProject.ViewModels.HomePage;
using OnlineMusicProject.ViewModels.PlaylistPage;
using OnlineMusicProject.ViewModels.SongPage;
using Org.BouncyCastle.Utilities.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineMusicProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly OnlineMusicDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public UsersController(UserManager<Users> userManager, OnlineMusicDBContext context, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            _context = context;
            _environment = environment;
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
        public async Task<IActionResult> HistoryOfListening(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = "Songs";
            }
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var histories = await _context.Histories
                   .Include(h => h.Songs).ThenInclude(h => h.Artists)
                   .Include(h => h.Albums).ThenInclude(h => h.Artists)
                   .Where(h => h.UserId == user.Id.ToString())
                   .OrderByDescending(h => h.PlayedAt)
                   .Take(5)
                   .ToListAsync();
                var filteredHistories = category == "Songs" ? histories.Where(h => h.AlbumId == null).ToList()
                        : category == "Albums" ? histories.Where(h => h.SongId == null).ToList()
                        : new List<Histories>();
                var model = new UserProfileViewModel
                {
                    User = user,
                    histories = filteredHistories,
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> HistoryOfListening(Guid? songId, Guid? albumId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                if (songId != null)
                {
                    return RedirectToAction("Details", "Songs", new { id = songId });
                }
                if (albumId != null)
                {
                    return RedirectToAction("Details", "Albums", new { albumId });
                }
                return BadRequest("User is not logged in and no valid ID is provided.");
            }

            if (songId != null)
            {
                var song = await _context.Songs.FindAsync(songId);
                if (song == null)
                {
                    return NotFound();
                }

                var historySong = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == user.Id && h.SongId == songId);
                if (historySong == null)
                {
                    var newHistory = new Histories
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
                    historySong.PlayedAt = DateTime.Now;
                    _context.Histories.Update(historySong);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Songs", new { id = songId });
            }

            if (albumId != null)
            {
                var album = await _context.Albums.FindAsync(albumId);
                if (album == null)
                {
                    return NotFound();
                }

                var historyAlbum = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == user.Id && h.AlbumId == albumId);
                if (historyAlbum == null)
                {
                    var newHistory = new Histories
                    {
                        HistoryId = Guid.NewGuid(),
                        UserId = user.Id,
                        AlbumId = albumId,
                        PlayedAt = DateTime.Now,
                    };
                    _context.Histories.Add(newHistory);
                }
                else
                {
                    historyAlbum.PlayedAt = DateTime.Now;
                    _context.Histories.Update(historyAlbum);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Albums", new { albumId });
            }
            return BadRequest("No songId or albumId provided.");
        }


        public async Task<IActionResult> RemoveFromHistories(Guid? songId, Guid? albumId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            var history = await _context.Histories
                          .FirstOrDefaultAsync(h => h.UserId == user.Id.ToString() &&
                           (h.SongId == songId || h.AlbumId == albumId));
            if (history != null)
            {
                _context.Histories.Remove(history);
                await _context.SaveChangesAsync();
                return RedirectToAction("HistoryOfListening");
            }
            return RedirectToAction("HistoryOfListening");
        }

        public IActionResult Contact() => View();

        public IActionResult Elements() => View();

        public async Task<ActionResult<IEnumerable<Artists>>> Artists(string searchQuery, int page = 1)
        {
            int pageSize = 6;
            var artists = _context.Artists.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                artists = artists.Where(s => s.ArtistName.Contains(searchQuery));
            }
            var artistslist = await artists.ToListAsync();
            var pageResult = PageResult.ToPaginatedList(artistslist, page, pageSize);
            ViewData["SearchQuery"] = searchQuery;
            return View(pageResult);
        } 
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Playlist()
        {
            var user = await userManager.GetUserAsync(User);
            List<Playlists> play = new List<Playlists>();
            List<playlistWithCounts> playlistSongsWithCounts = new List<playlistWithCounts>();
            if (user != null)
            {
                play = await _context.Playlists.Where(p => p.UserId == user.Id).OrderByDescending(p => p.PlaylistId).ToListAsync();
                foreach(var item in play)
                {
                    int count = await _context.PlaylistSongs
                                          .Where(ps => ps.PlaylistId == item.PlaylistId)
                                          .CountAsync();
                    playlistSongsWithCounts.Add(new playlistWithCounts
                    {
                        Playlists = item,
                        CountSongInPlaylist = count
                    });
                }
            }
            List<SongGenres> genres = await _context.SongGenres.ToListAsync();
            List<Artists> artists = await _context.Artists.ToListAsync();
            var model = new PlaylistViewModel
            {
                PlaylistItems = play,
                PlaylistItemsWithCounts = playlistSongsWithCounts,
                SongGenres = genres,
                Artists = artists
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Playlist(string playlistName, IFormFile avatar)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                Playlists playItem = new Playlists
                {
                    PlaylistId = Guid.NewGuid(),
                    PlaylistName = playlistName,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                };
                if (string.IsNullOrEmpty(playlistName))
                {
                    TempData["RequiredMsg"] = "Name is required";
                    return RedirectToAction("Playlist");
                }
                if (avatar != null && avatar.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "playlist-img", avatar.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }

                    playItem.PlaylistImage = "/img/playlist-img/" + avatar.FileName;
                }
                _context.Playlists.Add(playItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("PlayList");
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> addToPlaylist(Guid itemId, Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var playItems = await _context.Playlists.FirstOrDefaultAsync(pi => pi.PlaylistId == itemId);
                if (playItems != null)
                {
                    var exitingSong = await _context.PlaylistSongs.FirstOrDefaultAsync(es => es.PlaylistId == playItems.PlaylistId && es.SongId == songId);
                    if (exitingSong == null)
                    {
                        PlaylistSongs playlistSong = new PlaylistSongs
                        {
                            PlaylistId = playItems.PlaylistId,
                            SongId = songId,
                        };
                        _context.PlaylistSongs.Add(playlistSong);
                        await _context.SaveChangesAsync();
                        TempData["MsgToDetail"] = "Song added successfully!";

                        playItems.CreatedAt = DateTime.Now;
                        _context.Playlists.Update(playItems);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["MsgToDetail"] = "This song is already in the playlist!";
                    }
                }
            }
            return RedirectToAction("Details", "Songs", new { id = songId });
        }

        [HttpPost]
        public async Task<IActionResult> AddSongs(SongsViewModel model)
        {
            var user = await userManager.GetUserAsync(User);

            var nameSong = await _context.Songs.FirstOrDefaultAsync(s => s.NameSong == model.NameSong);
            var imageSong = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == model.ArtistId);

            if (nameSong != null)
            {
                TempData["MsgRequired"] = "This name already exists.";
                return RedirectToAction("Playlist");
            }

            if (user != null)
            {

                var song = new Songs
                {
                    SongId = Guid.NewGuid(),
                    NameSong = model.NameSong,
                    ImageSong = imageSong?.ArtistImage,
                    GenreId = model.GenreId,
                    ArtistId = model.ArtistId,
                    UserId = user.Id,
                    IsPublic = model.IsPublic,
                };
                if (model.FilePath != null && model.FilePath.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio", model.FilePath.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FilePath.CopyToAsync(stream);
                    }

                    song.FilePath = "/audio/ " + model.FilePath.FileName;
                }
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("PlayList");
            }
            return RedirectToAction("Login", "Account");
        }

    }
}