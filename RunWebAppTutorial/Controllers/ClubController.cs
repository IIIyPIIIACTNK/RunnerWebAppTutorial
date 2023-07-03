using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Migrations;
using RunWebAppTutorial.Models;
using RunWebAppTutorial.ViewModel;
using System.Diagnostics;

namespace RunWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository,IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository= clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club= await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserById();
            var createViewModel = new CreateClubViewModel()
            {
                AppUserId = currentUserId
            };
            return View(createViewModel);
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
                    AppUserId = clubViewModel.AppUserId,
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if(club == null) return View("Error");
            var editViewModel = new EditClubViewModel()
            {
               Title = club.Title,
               Description = club.Description,
               Address = club.Address,
               AddressId= club.AddressId,
               URL = club.Image,
               ClubCategory= club.ClubCategory,
            };
            return View(editViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubViewModel);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTraking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeleteImageAsync(userClub.Image);
                }
                catch
                {
                    ModelState.AddModelError("", "Failed to delete image");
                    return View(clubViewModel);
                }
                var photoUploadResult = await _photoService.AddPhotoAsync(clubViewModel.Image);

                var club = new Club()
                {
                    Id = id,
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = photoUploadResult.Url.ToString(),
                    AddressId = clubViewModel.AddressId,
                    Address = clubViewModel.Address
                };

                _clubRepository.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(clubViewModel);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetail = await _clubRepository.GetByIdAsync(id);
            if(clubDetail == null) { return View("Error"); }
            return View(clubDetail);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetail = await _clubRepository.GetByIdAsync(id);
            if(clubDetail == null) { return View("Error"); }
            try
            {
                await _photoService.DeleteImageAsync(clubDetail.Image);
                Debug.WriteLine($"Cloudinary image delete code checked \nLink: {clubDetail.Image}");
            }
            catch
            {
                ModelState.AddModelError("", "Failed to delete image");
                return View(clubDetail);
            }
            _clubRepository.Delete(clubDetail); 
            return RedirectToAction("Index");
        }
    }
}
