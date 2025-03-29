using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMusicProject.Models;
using OnlineMusicProject.Services.Pagination;
using OnlineMusicProject.ViewModels.AlbumsPage;
using OnlineMusicProject.ViewModels.PlaylistPage;
using OnlineMusicProject.ViewModels.SongPage;

namespace OnlineMusicProject.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly OnlineMusicDBContext _context;
        private readonly UserManager<Users> userManager;

        public AlbumsController(OnlineMusicDBContext context, UserManager<Users> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(Guid albumId, Guid? songId)
        {
            var albums = await _context.Albums.Include(a => a.Artists).Include(a => a.User).FirstOrDefaultAsync(a => a.AlbumId == albumId);
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
			if (albums != null)
            {
				var songInAlbums = await _context.AlbumSongs
					.Include(asg => asg.Albums)
					.Include(asg => asg.Songs)
					.ThenInclude(a => a.Artists)
					.Where(asg => asg.AlbumId == albumId)
					.ToListAsync();
				AlbumSongs singleSong = null;
                if(songId != null && songId != Guid.Empty)
                {
					singleSong = await _context.AlbumSongs
				    .Include(asg => asg.Albums)
					.Include(asg => asg.Songs)
					.ThenInclude(a => a.Artists)
					   .FirstOrDefaultAsync(ps => ps.AlbumId == albums.AlbumId && ps.SongId == songId);
				}
				if (singleSong == null && songInAlbums.Any())
				{
					singleSong = songInAlbums.First();
				}
				if (songInAlbums == null || singleSong == null)
				{
					return RedirectToAction("Index", "Home");
				}

				var model = new AlbumsAndSongsViewModel
				{
					album = albums,
					albumSongs = songInAlbums,
					SingleAlbumSongs = singleSong,
					PlaylistItems = playitems,
					PlaylistItemsWithCounts = playlistSongsWithCounts
					
				};

				return View(model);
			}
			return RedirectToAction("Index","Home");
		}

		public async Task<IActionResult> List(string searchQuery, int page = 1)
		{
			int pageSize = 6;
			var albums = _context.Albums.Include(s => s.Artists).AsQueryable();
			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				albums = albums.Where(s => s.Title.Contains(searchQuery));
			}
			var songslist = await albums.ToListAsync();
			var pageResult = PageResult.ToPaginatedList(songslist, page, pageSize);
			ViewData["SearchQuery"] = searchQuery;
			return View(pageResult);
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
			return RedirectToAction("Details", new { albumId = itemId });
		}
		[HttpPost]
		public async Task<IActionResult> CreatePlaylist(Guid albumId, string playlistName, Guid songId)
		{
			var user = await userManager.GetUserAsync(User);

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
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
			return RedirectToAction("Details", new { albumId});
		}

		public async Task<IActionResult> AddAlbumToPlaylist(Guid albumId)
		{
			var user = await userManager.GetUserAsync(User);
			var album = await _context.Albums.Include(s => s.Artists).FirstOrDefaultAsync(al => al.AlbumId == albumId);
			if (user != null && album != null)
			{
				var existingPlaylist = await _context.Playlists.FirstOrDefaultAsync(p => p.PlaylistName == album.Title);
				if (existingPlaylist == null)
				{
					Playlists playlists = new Playlists
					{
						PlaylistId = Guid.NewGuid(),
						PlaylistName = album.Title,
						PlaylistImage = album.Album_Image,
						UserId = user.Id,
					};
					_context.Playlists.Add(playlists);
					await _context.SaveChangesAsync();
                    TempData["MsgToDetail"] = "Albumn added successfully!";
				}
				else
				{
					TempData["MsgToDetail"] = "This albumn is already in the playlist!";
				}
			}
			return RedirectToAction("Details", new { albumId });
		}
	}
}
