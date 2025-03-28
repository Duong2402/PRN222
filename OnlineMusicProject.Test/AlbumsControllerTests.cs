using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OnlineMusicProject.Controllers;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.AlbumsPage;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineMusicProject.Test
{
    [TestFixture]
    public class AlbumsControllerTests
    {
        private AlbumsController _controller;
        private Mock<UserManager<Users>> _userManagerMock;
        private OnlineMusicDBContext _context;
        private SqliteConnection _connection;
        private string _userId;
        private Guid _artistId;
        private Guid _albumId;
        private Guid _songId;
        private Guid _playlistId;

        [SetUp]
        public void Setup()
        {
            // Set up SQLite in-memory database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<OnlineMusicDBContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new OnlineMusicDBContext(options);
            _context.Database.EnsureCreated();

            // Seed initial data
            var user = new Users
            {
                Id = "test-user-id",
                UserName = "testuser@onesound.com",
                Email = "testuser@onesound.com",
                FullName = "Test User"
            };
            var artist = new Artists
            {
                ArtistId = Guid.NewGuid(),
                ArtistName = "Test Artist",
                ArtistImage = "/images/test-artist.jpg"
            };
            var genre = new SongGenres
            {
                GenreId = Guid.NewGuid(),
                Name = "Pop"
            };
            var album = new Albums
            {
                AlbumId = Guid.NewGuid(),
                Title = "Test Album",
                Album_Image = "/images/test-album.jpg",
                CreatedAt = DateTime.UtcNow,
                ArtistId = artist.ArtistId,
                UserId = user.Id
            };
            var song = new Songs
            {
                SongId = Guid.NewGuid(),
                NameSong = "Test Song",
                ArtistId = artist.ArtistId,
                GenreId = genre.GenreId,
                FilePath = "/uploads/audio/test.mp3",
                ImageSong = "/uploads/images/test.jpg",
                NumberOfListeners = 0,
                Lyrics = "Test lyrics"
            };
            var albumSong = new AlbumSongs
            {
                AlbumId = album.AlbumId,
                SongId = song.SongId
            };
            var playlist = new Playlists
            {
                PlaylistId = Guid.NewGuid(),
                PlaylistName = "Test Playlist",
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                PlaylistImage = "/uploads/images/playlist.jpg"
            };

            _context.Users.Add(user);
            _context.Artists.Add(artist);
            _context.SongGenres.Add(genre);
            _context.Albums.Add(album);
            _context.Songs.Add(song);
            _context.AlbumSongs.Add(albumSong);
            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            // Store IDs for use in tests
            _userId = user.Id;
            _artistId = artist.ArtistId;
            _albumId = album.AlbumId;
            _songId = song.SongId;
            _playlistId = playlist.PlaylistId;

            // Set up UserManager mock
            var store = new Mock<IUserStore<Users>>();
            _userManagerMock = new Mock<UserManager<Users>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            // Set up the controller with a mocked user context
            _controller = new AlbumsController(_context, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, _userId),
                            new Claim(ClaimTypes.Role, "User")
                        }, "mock"))
                    }
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _connection.Close();
            _controller?.Dispose();
        }

        // Tests for Details
        [Test]
        public async Task Details_ValidAlbumWithSongId_ReturnsViewWithModel()
        {
            var result = await _controller.Details(_albumId, _songId);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as AlbumsAndSongsViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_albumId, model.album.AlbumId);
            Assert.AreEqual(1, model.albumSongs.Count);
            Assert.AreEqual(_songId, model.SingleAlbumSongs.SongId);
            Assert.AreEqual(1, model.PlaylistItems.Count);
            Assert.AreEqual(_playlistId, model.PlaylistItems[0].PlaylistId);
            Assert.AreEqual(1, model.PlaylistItemsWithCounts.Count);
            Assert.AreEqual(0, model.PlaylistItemsWithCounts[0].CountSongInPlaylist);
        }

        [Test]
        public async Task Details_ValidAlbumWithoutSongId_ReturnsViewWithFirstSong()
        {
            var result = await _controller.Details(_albumId, null);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as AlbumsAndSongsViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_albumId, model.album.AlbumId);
            Assert.AreEqual(1, model.albumSongs.Count);
            Assert.AreEqual(_songId, model.SingleAlbumSongs.SongId); // First song in the album
        }

        [Test]
        public async Task Details_AlbumNotFound_ReturnsRedirectToHome()
        {
            var invalidAlbumId = Guid.NewGuid();

            var result = await _controller.Details(invalidAlbumId, null);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [Test]
        public async Task Details_AlbumWithNoSongs_ReturnsRedirectToHome()
        {
            // Create an album with no songs
            var emptyAlbum = new Albums
            {
                AlbumId = Guid.NewGuid(),
                Title = "Empty Album",
                Album_Image = "/images/empty-album.jpg",
                CreatedAt = DateTime.UtcNow,
                ArtistId = _artistId,
                UserId = _userId
            };
            _context.Albums.Add(emptyAlbum);
            await _context.SaveChangesAsync();

            var result = await _controller.Details(emptyAlbum.AlbumId, null);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [Test]
        public async Task CreatePlaylist_UnauthenticatedUser_ReturnsRedirectToLogin()
        {
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((Users)null);

            var playlistName = "New Playlist";

            var result = await _controller.CreatePlaylist(_albumId, playlistName, _songId);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Login", redirectResult.ActionName);
            Assert.AreEqual("Account", redirectResult.ControllerName);
        }
    }
}