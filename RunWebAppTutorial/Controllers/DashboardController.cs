using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
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
    }
}
