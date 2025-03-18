using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.PlaylistPage;
using OnlineMusicProject.ViewModels.SongPage;
using Org.BouncyCastle.Utilities;

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

        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(Guid playlistId, Guid? songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<playlistWithCounts> playlistSongsWithCounts = new List<playlistWithCounts>();
            var playitems = await _context.Playlists
                                  .Where(pi => pi.UserId == user.Id)
                                  .GroupBy(pi => pi.PlaylistName)
                                  .Select(group => group.First())
                                  .ToListAsync();

            foreach (var playlist in playitems)
            {
                int countSongs = await _context.PlaylistSongs
                                               .Where(ps => ps.PlaylistId == playlist.PlaylistId)
                                               .CountAsync();
                playlistSongsWithCounts.Add(new playlistWithCounts
                {
                    Playlists = playlist,
                    CountSongInPlaylist = countSongs
                });
            }

            var play = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id && p.PlaylistId == playlistId);
            if (play != null)
            {
                var songsInPlaylist = await _context.PlaylistSongs
                    .Include(ps => ps.Songs)
                    .ThenInclude(ps => ps.Artists)
                    .Where(ps => ps.PlaylistId == play.PlaylistId)
                    .ToListAsync();
				PlaylistSongs singleSong = null;
                if (songId != null && songId != Guid.Empty)
                {
                    singleSong = await _context.PlaylistSongs
                        .Include(ps => ps.Songs)
                        .ThenInclude(song => song.Artists)
                        .FirstOrDefaultAsync(ps => ps.PlaylistId == play.PlaylistId && ps.SongId == songId);
                }

                if (singleSong == null && songsInPlaylist.Any())
                {
                    singleSong = songsInPlaylist.First();
                }

                if (songsInPlaylist == null || singleSong == null)
                {
                    return RedirectToAction("Playlist", "Users");
                }

                var model = new SongsOfPlaylistViewModel
                {
                    UserName = user.FullName,
                    PlaylistItems = playitems,
                    PlaylistItemsWithCounts = playlistSongsWithCounts,
                    playlistItem = play,
                    SinglePlaylistSongs = singleSong,
                    playlistSongs = songsInPlaylist,
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

        public async Task<IActionResult> RemoveSongFromPlaylist(Guid playlistId, Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            var play = await _context.Playlists.FirstOrDefaultAsync(p => p.UserId == user.Id && p.PlaylistId == playlistId);
            if (play != null)
            {
                var songPlaylist = await _context.PlaylistSongs
                                      .FirstOrDefaultAsync(ps => ps.SongId == songId && ps.PlaylistId == play.PlaylistId);
                if (songPlaylist == null) return RedirectToAction("Details", new { playlistId });
                _context.PlaylistSongs.Remove(songPlaylist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { playlistId });
            }
            return RedirectToAction("Playlist", "Users");
        }

        public async Task<IActionResult> addSongToPlaylist(Guid playlistId, Guid itemId, Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var playItems = await _context.Playlists.FirstOrDefaultAsync(pi => pi.PlaylistId == playlistId);
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
            return RedirectToAction("Details", new { playlistId = itemId });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(Guid playlistId, string playlistName, Guid songId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(playlistName))
            {
                TempData["RequiredMsg"] = "Name is required";
                return RedirectToAction("Details", new { playlistId });
            }

            Playlists playItem = new Playlists
            {
                PlaylistId = Guid.NewGuid(),
                PlaylistName = playlistName,
                UserId = user.Id,
                CreatedAt = DateTime.Now,
            };

            _context.Playlists.Add(playItem);
            await _context.SaveChangesAsync();

            PlaylistSongs playlistSongs = new PlaylistSongs
            {
                PlaylistId = playItem.PlaylistId,
                SongId = songId
            };

            _context.PlaylistSongs.Add(playlistSongs);
            await _context.SaveChangesAsync();
            TempData["MsgToDetail"] = "Song added successfully!";
            return RedirectToAction("Details", new { playlistId });
        }
	}
}
