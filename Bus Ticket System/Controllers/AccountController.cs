using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_System.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index2", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);

                }
            }

            return View(registerViewModel);
        }

        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index2", "home");
        }
        [Route("logoutCheck")]

        public async Task<IActionResult> LogoutCheck()
        {
            await Logout();
            return RedirectToAction("index2", "home");
        }


        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }



        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(loginViewModel.Name, loginViewModel.Password,
                    loginViewModel.RememberMe, false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    // redirect to admin panel 
                    if (result.Succeeded)
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index2", "AdminPanel");
                        }
                        else
                        {
                            return RedirectToAction("index2" +
                                "", "home");
                        }
                    else
                        ModelState.AddModelError(String.Empty, "Invalid Login Attempt");
                }

            }

            return View(loginViewModel);
        }


    }
}
