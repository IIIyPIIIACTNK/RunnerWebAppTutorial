using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDBContext _context;
        public RaceController(ApplicationDBContext context) 
        { 
            _context= context;
        }
        public IActionResult Index()
        {
            var races = _context.Races.ToList();
            return View(races);
        }

        public IActionResult Detail(int id)
        {
            Race club = _context.Races.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
            return View(club);
        }
    }
}
