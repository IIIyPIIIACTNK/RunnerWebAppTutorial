using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Interfaces;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach(var user in users)
            {
                result.Add(new UserViewModel()
                {
                    Id= user.Id,
                    UserName = user.UserName,
                    Mileage= user.Mileage,
                    Pace= user.Pace
                });
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            var userViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Mileage = user.Mileage,
                Pace = user.Pace,
            };
            return View(userViewModel);
        }
    }
}
