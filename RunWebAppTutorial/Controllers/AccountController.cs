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
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if(!ModelState.IsValid) {
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if(user != null){

                var passwordCheck = await _userManager.CheckPasswordAsync(user,loginVM.Password);
                if(passwordCheck) 
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                    TempData["Error"] = "Wrong credetenrials. Try again";
                    return View(loginVM);
                }
            }
            TempData["Error"] = "Wrong credetenrials. Try again";
            return View(loginVM);
        }
    }
}
