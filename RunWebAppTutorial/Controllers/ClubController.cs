﻿using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Migrations;

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
    }
}
