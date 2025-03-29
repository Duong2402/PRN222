using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OnlineMusicProject.Controllers;
using OnlineMusicProject.Models;
using OnlineMusicProject.Test.Helpers;
using OnlineMusicProject.ViewModels;
using OnlineMusicProject.ViewModels.HomePage;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineMusicProject.Test
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<UserManager<Users>> _userManagerMock;
        private OnlineMusicDBContext _context;
        private SqliteConnection _connection;
        private string _userId;
        private Guid _songId;
        private Guid _playlistId;
        private Guid _albumId;
        private string _category;

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
            var song = new Songs
            {
                SongId = Guid.NewGuid(),
                NameSong = "Test Song",
                ArtistId = artist.ArtistId,
                GenreId = genre.GenreId,
                Lyrics = "Test lyrics",
                FilePath = "/uploads/audio/test.mp3",
                ImageSong = "/uploads/images/test.jpg",
                NumberOfListeners = 0
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
            _context.Songs.Add(song);
            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            // Store IDs for use in tests
            _userId = user.Id;
            _songId = song.SongId;
            _playlistId = playlist.PlaylistId;

            // Set up UserManager mock
            var store = new Mock<IUserStore<Users>>();
            _userManagerMock = new Mock<UserManager<Users>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<Users>())).ReturnsAsync(IdentityResult.Success);

            // Set up the controller with a mocked user context
            _controller = new UsersController(_userManagerMock.Object, _context)
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

        // Tests for Profile (GET)
        [Test]
        public void Profile_Get_ReturnsView()
        {
            var result = _controller.Profile();

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

      
        [Test]
        public async Task Profile_Post_ValidUserWithoutAvatar_ReturnsRedirectToProfile()
        {
            var userUpdate = new Users
            {
                FullName = "Updated User",
                PhoneNumber = "1234567890"
            };

            var result = await _controller.Profile(userUpdate, null);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Profile", redirectResult.ActionName);

            var updatedUser = await _context.Users.FindAsync(_userId);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Updated User", updatedUser.FullName);
            Assert.AreEqual("1234567890", updatedUser.PhoneNumber);
            Assert.IsNull(updatedUser.Avatar);
        }

        [Test]
        public async Task Profile_Post_InvalidModel_ReturnsViewWithModel()
        {
            var userUpdate = new Users
            {
                FullName = "", // Invalid: FullName might be required
                PhoneNumber = "1234567890"
            };
            _controller.ModelState.AddModelError("FullName", "The FullName field is required.");

            var result = await _controller.Profile(userUpdate, null);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(userUpdate, viewResult.Model);
        }

        // Tests for HistoryOfListening (GET)
        [Test]
        public async Task HistoryOfListening_Get_AuthenticatedUser_ReturnsViewWithModel()
        {
            // Add a history entry
            var history = new Histories
            {
                HistoryId = Guid.NewGuid(),
                UserId = _userId,
                SongId = _songId,
                PlayedAt = DateTime.UtcNow
            };
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            var result = await _controller.HistoryOfListening(_category);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as UserProfileViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_userId, model.User.Id);
            Assert.AreEqual(1, model.histories.Count);
            Assert.AreEqual(_songId, model.histories[0].SongId);
        }

        [Test]
        public async Task HistoryOfListening_Get_UnauthenticatedUser_ReturnsRedirectToLogin()
        {
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((Users)null);

            var result = await _controller.HistoryOfListening(_category);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Login", redirectResult.ActionName);
            Assert.AreEqual("Account", redirectResult.ControllerName);
        }

        // Tests for HistoryOfListening (POST)
        [Test]
        public async Task HistoryOfListening_Post_NewHistory_ReturnsRedirectToSongDetails()
        {
            var result = await _controller.HistoryOfListening(_songId, _albumId);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Songs", redirectResult.ControllerName);
            Assert.AreEqual(_songId, redirectResult.RouteValues["id"]);

            var history = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == _userId && h.SongId == _songId);
            Assert.IsNotNull(history);
            Assert.IsNotNull(history.PlayedAt);
            var timeDifference = DateTime.UtcNow - history.PlayedAt.Value;
            Assert.IsTrue(timeDifference.TotalSeconds < 5); // PlayedAt should be recent
        }

        [Test]
        public async Task HistoryOfListening_Post_UpdateExistingHistory_ReturnsRedirectToSongDetails()
        {
            // Add an existing history entry
            var history = new Histories
            {
                HistoryId = Guid.NewGuid(),
                UserId = _userId,
                SongId = _songId,
                PlayedAt = DateTime.UtcNow.AddDays(-1)
            };
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            var result = await _controller.HistoryOfListening(_songId,_albumId);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Songs", redirectResult.ControllerName);

            var updatedHistory = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == _userId && h.SongId == _songId);
            Assert.IsNotNull(updatedHistory);
            Assert.IsNotNull(updatedHistory.PlayedAt);
            var timeDifference = DateTime.UtcNow - updatedHistory.PlayedAt.Value;
            Assert.IsTrue(timeDifference.TotalSeconds < 5); // PlayedAt should be updated
        }

        [Test]
        public async Task HistoryOfListening_Post_SongNotFound_ReturnsNotFound()
        {
            var invalidSongId = Guid.NewGuid();
            var invalidAlbumId = Guid.NewGuid();

            var result = await _controller.HistoryOfListening(invalidSongId, invalidAlbumId);

            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        // Tests for RemoveFromHistories
        [Test]
        public async Task RemoveFromHistories_ValidHistory_ReturnsRedirectToHistoryOfListening()
        {
            // Add a history entry
            var history = new Histories
            {
                HistoryId = Guid.NewGuid(),
                UserId = _userId,
                SongId = _songId,
                PlayedAt = DateTime.UtcNow
            };
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            var result = await _controller.RemoveFromHistories(_songId, _albumId);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("HistoryOfListening", redirectResult.ActionName);

            var historyEntry = await _context.Histories.FirstOrDefaultAsync(h => h.UserId == _userId && h.SongId == _songId);
            Assert.IsNull(historyEntry);
        }

        [Test]
        public async Task RemoveFromHistories_HistoryNotFound_ReturnsRedirectToHistoryOfListening()
        {
            var result = await _controller.RemoveFromHistories(_songId, _albumId);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("HistoryOfListening", redirectResult.ActionName);
        }

        // Tests for Playlist (GET)
        [Test]
        public async Task Playlist_Get_AuthenticatedUser_ReturnsViewWithModel()
        {
            // Add a playlist song entry
            var playlistSong = new PlaylistSongs
            {
                PlaylistId = _playlistId,
                SongId = _songId
            };
            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();

            var result = await _controller.Playlist();

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as PlaylistViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.PlaylistItems.Count);
            Assert.AreEqual(_playlistId, model.PlaylistItems[0].PlaylistId);
            Assert.AreEqual(1, model.PlaylistItemsWithCounts.Count);
            Assert.AreEqual(1, model.PlaylistItemsWithCounts[0].CountSongInPlaylist);
        }
    }
}