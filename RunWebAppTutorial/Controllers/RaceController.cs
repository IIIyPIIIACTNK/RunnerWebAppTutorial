﻿using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Data;

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
    }
}
