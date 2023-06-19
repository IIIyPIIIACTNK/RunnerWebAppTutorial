using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Migrations;
using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDBContext _context;
        public ClubController(ApplicationDBContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Detail(int id)
        {
            Club club= _context.Clubs.Include(a=>a.Address).FirstOrDefault(c => c.Id==id);
            return View(club);
        }
    }
}
