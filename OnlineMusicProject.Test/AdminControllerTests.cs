using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OnlineMusicProject.Controllers;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.AdminPage;
using System;
using System.Linq;
using System.Threading.Tasks;
using OnlineMusicProject.Test.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMusicProject.Test
{
    [TestFixture]
    public class AdminControllerTests
    {
        private AdminController _controller;
        private Mock<UserManager<Users>> _userManagerMock;
        private OnlineMusicDBContext _context;
        private SqliteConnection _connection;

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
            var user = new Users
            {
                Id = "test-user-id",
                UserName = "testuser@onesound.com",
                Email = "testuser@onesound.com",
                FullName = "Test User"
            };

            _context.Artists.Add(artist);
            _context.SongGenres.Add(genre);
            _context.Users.Add(user);
            _context.SaveChanges();

            // Set up UserManager mock
            var store = new Mock<IUserStore<Users>>();
            _userManagerMock = new Mock<UserManager<Users>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.Users).Returns(_context.Users.AsQueryable());

            _controller = new AdminController(_userManagerMock.Object, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _connection.Close();
            _controller?.Dispose();
        }

        // Tests for CreateAccount
        [Test]
        public async Task CreateAccount_ValidUser_ReturnsRedirectToManageAccounts()
        {
            var model = new CreateAccountViewModel
            {
                Email = "newuser@onesound.com",
                FullName = "New User",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Role = "User"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var result = await _controller.CreateAccount(model);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("ManageAccounts", redirectResult.ActionName);

            _userManagerMock.Verify();
        }

        [Test]
        public async Task CreateAccount_InvalidUser_ReturnsViewWithModel()
        {
            var model = new CreateAccountViewModel
            {
                Email = "", // Invalid: Email is required
                FullName = "New User",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Role = "User"
            };

            _controller.ModelState.AddModelError("Email", "The Email field is required.");

            var result = await _controller.CreateAccount(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task CreateAccount_PasswordMismatch_ReturnsViewWithModel()
        {
            var model = new CreateAccountViewModel
            {
                Email = "newuser@onesound.com",
                FullName = "New User",
                Password = "Test@123",
                ConfirmPassword = "Different@123", // Passwords don't match
                Role = "User"
            };

            var result = await _controller.CreateAccount(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(string.Empty));
            Assert.AreEqual("Passwords do not match.", viewResult.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        // Tests for EditAccount
        [Test]
        public async Task EditAccount_ValidUser_ReturnsRedirectToManageAccounts()
        {
            var userId = "test-user-id";
            var existingUser = new Users
            {
                Id = userId,
                UserName = "olduser@onesound.com",
                Email = "olduser@onesound.com",
                FullName = "Old User"
            };
            var model = new EditAccountViewModel
            {
                Id = userId,
                Email = "newuser@onesound.com",
                FullName = "New User"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                .ReturnsAsync(existingUser)
                .Verifiable();
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<Users>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var result = await _controller.EditAccount(model);

            Assert.IsNotNull(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("ManageAccounts", redirectResult.ActionName);

            _userManagerMock.Verify();
        }

        [Test]
        public async Task EditAccount_InvalidModel_ReturnsViewWithModel()
        {
            var model = new EditAccountViewModel
            {
                Id = "test-user-id",
                Email = "", // Invalid: Email is required
                FullName = "New User"
            };

            _controller.ModelState.AddModelError("Email", "The Email field is required.");

            var result = await _controller.EditAccount(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task EditAccount_UserNotFound_ReturnsNotFound()
        {
            var model = new EditAccountViewModel
            {
                Id = "non-existent-user-id",
                Email = "newuser@onesound.com",
                FullName = "New User"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(model.Id))
                .ReturnsAsync((Users)null)
                .Verifiable();

            var result = await _controller.EditAccount(model);

            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);

            _userManagerMock.Verify();
        }

        [Test]
        public async Task CreateSong_InvalidSong_ReturnsViewWithModel()
        {
            var model = new CreateSongViewModel
            {
                NameSong = "", // Invalid: NameSong is required
                ArtistId = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Lyrics = "Test lyrics",
                AudioFile = null, // Invalid: AudioFile is required
                ImageFile = new MockFormFile("test.jpg", "fake image content")
            };

            _controller.ModelState.AddModelError("NameSong", "The NameSong field is required.");
            _controller.ModelState.AddModelError("AudioFile", "The AudioFile field is required.");

            var result = await _controller.CreateSong(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task EditSong_SongNotFound_ReturnsNotFound()
        {
            var model = new EditSongViewModel
            {
                SongId = Guid.NewGuid(),
                NameSong = "Updated Song",
                ArtistId = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Lyrics = "Updated lyrics",
                FilePath = "/uploads/audio/test.mp3",
                ImageSong = "/uploads/images/test.jpg"
            };

            var result = await _controller.EditSong(model);

            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Test]
        public async Task CreatePlaylist_InvalidPlaylist_ReturnsViewWithModel()
        {
            var model = new CreatePlaylistViewModel
            {
                PlaylistName = "", // Invalid: PlaylistName is required
                UserId = "test-user-id",
                PlaylistImage = new MockFormFile("playlist.jpg", "fake image content")
            };

            _controller.ModelState.AddModelError("PlaylistName", "The PlaylistName field is required.");

            var result = await _controller.CreatePlaylist(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task EditPlaylist_InvalidPlaylist_ReturnsViewWithModel()
        {
            var playlist = new Playlists
            {
                PlaylistId = Guid.NewGuid(),
                PlaylistName = "Old Playlist",
                UserId = "test-user-id",
                CreatedAt = DateTime.UtcNow,
                PlaylistImage = "/uploads/images/old.jpg"
            };
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            var model = new EditPlaylistViewModel
            {
                PlaylistId = playlist.PlaylistId,
                PlaylistName = "", // Invalid: PlaylistName is required
                UserId = "test-user-id",
                PlaylistImage = playlist.PlaylistImage
            };

            _controller.ModelState.AddModelError("PlaylistName", "The PlaylistName field is required.");

            var result = await _controller.EditPlaylist(model);

            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }
    }
}