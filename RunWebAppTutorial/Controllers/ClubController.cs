using Microsoft.AspNetCore.Mvc;

namespace RunWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
