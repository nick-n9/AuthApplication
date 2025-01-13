using AuthApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _um;
        private readonly SignInManager<AppUser> _sm;

        public AccountController(UserManager<AppUser> um, SignInManager<AppUser> sm)
        {
            _um = um;
            _sm = sm;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser 
                { 
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                var result = await _um.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _sm.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _um.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _sm.PasswordSignInAsync
                        (
                            user,
                            model.Password,
                            isPersistent: false,
                            lockoutOnFailure: false
                        );

                    if (result.Succeeded)
                    {
                        // Storing user data in session
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetString("Email", user.Email);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found, Please Register.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
