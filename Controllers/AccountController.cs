using Microsoft.AspNetCore.Mvc;
using RecyclingHub.Models.ViewModels;
using RecyclingHub.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
namespace RecyclingHub.Controllers
{
    public class AccountController(UserManager<AppUser> userManager ,
        SignInManager<AppUser> signInManager, INotyfService notyfService) : Controller
    {
        readonly UserManager<AppUser> _userManager = userManager;
        readonly SignInManager<AppUser> _signInManager = signInManager;
        readonly INotyfService NotyfService =  notyfService;

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.Username);
                //check if username is correct
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty,
                        "Username does not exist");
                    return View(loginViewModel);
                }
                var result = await  _signInManager.PasswordSignInAsync(user , loginViewModel.Password,false,false);
                //check if password is correct
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty,
                        "Password is incorrect");
                    return View(loginViewModel);
                }
                NotyfService.Success("Login Successfull");
                return Redirect("/");
            }
            ModelState.AddModelError(string.Empty,"Enter missing details");
            NotyfService.Error("Login failed");
            return View(loginViewModel);
        }
        public async Task<RedirectResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/account/login");
        }
    }
}
