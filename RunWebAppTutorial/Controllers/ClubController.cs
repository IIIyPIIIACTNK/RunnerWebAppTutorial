using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Migrations;
using RunWebAppTutorial.Models;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository,IPhotoService photoService)
        {
            _clubRepository= clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club= await _clubRepository.GetById(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
            {
            if(ModelState.IsValid)
            {
                var photoUploadResult = await _photoService.AddPhotoAsync(clubViewModel.Image);

                var club = new Club()
                {
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Address= clubViewModel.Address,
                    Image = photoUploadResult.Url.ToString(),
                    ClubCategory= clubViewModel.ClubCategory,
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Image upload failed");
            }
            return View();
        }
    }
}
