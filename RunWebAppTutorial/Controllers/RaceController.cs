using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        public RaceController(IRaceRepository raceRepository) 
        { 
            _raceRepository= raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race club = await _raceRepository.GetById(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if(!ModelState.IsValid) 
            {
                return View(race);
            }

            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }
}
