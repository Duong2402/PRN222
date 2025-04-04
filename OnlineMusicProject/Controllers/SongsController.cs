﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.Services.Pagination;
using OnlineMusicProject.ViewModels.SongPage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineMusicProject.Controllers
{
    public class SongsController : Controller
    {
        private readonly OnlineMusicDBContext _context;
        private readonly UserManager<Users> userManager;

        public SongsController(OnlineMusicDBContext context, UserManager<Users> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Songs song = await _context.Songs.Include(p => p.Artists)
                                       .FirstOrDefaultAsync(p => p.SongId == id);

			if (song != null)
            {
                song.NumberOfListeners += 1;
                _context.Songs.Update(song);
                await _context.SaveChangesAsync();
            }
            var user = await userManager.GetUserAsync(User);
            List<Playlists> playitems = new List<Playlists>();
            List<playlistWithCounts> playlistSongsWithCounts = new List<playlistWithCounts>();
            if (user != null)
            {
                playitems = await _context.Playlists
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
            }
			var SongsByGenres = await _context.Songs.Include(s => s.Artists).Where(s => s.GenreId == song.GenreId && s.SongId != id && song.IsPublic == true).ToListAsync();
            var model = new SongPlaylistViewModel
            {
                Song = song,
				Songs = SongsByGenres,
                PlaylistItems = playitems,
                PlaylistItemsWithCounts = playlistSongsWithCounts
            };

            return View(model);
        }
        public async Task<IActionResult> SongList(string searchQuery, int page = 1)
        {
			int pageSize = 6;
			var songs = _context.Songs.Include(s => s.Artists).Where(s => s.IsPublic == true).AsQueryable();
			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
                songs = songs.Where(s => s.NameSong.Contains(searchQuery));
			}
			var songslist = await songs.ToListAsync();
			var pageResult = PageResult.ToPaginatedList(songslist, page, pageSize);
            ViewData["SearchQuery"] = searchQuery;
            return View(pageResult);
		}
		[HttpPost]
        public async Task<IActionResult> CreatePlaylist(string playlistName, Guid songId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(playlistName))
            {
                TempData["RequiredMsg"] = "Name is required";
                return RedirectToAction("Details", new { id = songId });
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
            return RedirectToAction("Details", new { id = songId });
        }

	}

}
