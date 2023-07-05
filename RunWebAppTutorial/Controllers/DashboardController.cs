using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.Models;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserImage(AppUser appUser,EditUserDashboardViewModel editViewModel, ImageUploadResult imageUploadResult)
        {
            appUser.Id= editViewModel.Id;
            appUser.Pace = editViewModel.Pace;
            appUser.Mileage= editViewModel.Mileage;
            appUser.City= editViewModel.City;
            appUser.State= editViewModel.State;
            appUser.ProfileImageUrl= imageUploadResult.Url.ToString();
            
        }
        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userDashboard = new DashboardViewModel()
            {
                UserClubs = userClubs,
                UserRaces = userRaces
            };
            return View(userDashboard);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(currentUserId);
            if(user == null) 
            {
                return View("Error");
            }
            var userViewModel = new EditUserDashboardViewModel()
            {
                Id = currentUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editViewModel)
        {
            if(!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Failed to edit user profile");
                return View(editViewModel);
            }

            var user = await _dashboardRepository.GetUserByIdNoTracking(editViewModel.Id);

            if(user == null)
            {
                return View("Error");
            }

            if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editViewModel.Image);

                MapUserImage(user,editViewModel ,photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeleteImageAsync(user.ProfileImageUrl);
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", "Failed to delete photo");
                    return View(editViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(editViewModel.Image);

                MapUserImage(user, editViewModel, photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
        }
    }
}
