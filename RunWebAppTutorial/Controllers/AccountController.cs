using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunWebAppTutorial.Data;
using RunWebAppTutorial.Models;
using RunWebAppTutorial.ViewModel;

namespace RunWebAppTutorial.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDBContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if(user != null){

                var passwordCheck = await _userManager.CheckPasswordAsync(user,loginViewModel.Password);
                if(passwordCheck) 
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                    TempData["Error"] = "Wrong credetenrials. Try again";
                    return View(loginViewModel);
                }
            }
            TempData["Error"] = "Wrong credetenrials. Try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This Email is already registered";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
            };
            var newUserResponce = await _userManager.CreateAsync(newUser,registerViewModel.Password);

            if(newUserResponce.Succeeded) 
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Index", "Race");
            }

            TempData["Error"] = newUserResponce.Errors.First().Description;
            return View(registerViewModel);
        }

        [HttpGet] 
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }
    }
}
