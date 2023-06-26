using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Models;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository,IPhotoService photoService) 
        { 
            _raceRepository= raceRepository;
            _photoService = photoService;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
        {
            if(ModelState.IsValid) 
            {
                var photoUploadResult = await _photoService.AddPhotoAsync(raceViewModel.Image);

                var race = new Race()
                {
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Address = raceViewModel.Address,
                    Image = photoUploadResult.Url.ToString(),
                    RaceCategory = raceViewModel.RaceCategory
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(raceViewModel);
        }
    }
}
