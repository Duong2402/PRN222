using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels;

namespace OnlineMusicProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<Users> userManager;

        public UsersController(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

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
        public IActionResult Blog()
        {
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
        public IActionResult Event()
        {
            return View();
        }
    }
}
