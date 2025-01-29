using Microsoft.AspNetCore.Mvc;

namespace OnlineMusicProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
