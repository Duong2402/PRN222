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

        [Authorize(Roles = "User, Admin")]
        public IActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Profile(Users u)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if(user != null)
                {
                    user.FullName = u.FullName;
                    var result = await userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(u);
        }
        public IActionResult AlbumsStore()
        {
            return View();
        }
        public IActionResult HistoryOfListening()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> HistoryOfListening(Guid songId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("./Account/Login");
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
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Elements()
        {
            return View();
        }
        public async Task<ActionResult<IEnumerable<Artists>>> Artists()
        {
            List<Artists> artists = await _context.Artists.ToListAsync();
            return View(artists);
        }
    }
}
