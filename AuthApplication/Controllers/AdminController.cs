using AuthApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _um;
        public AdminController(UserManager<AppUser> um)
        {
            _um = um;
        }

        [HttpGet]
        public async Task<IActionResult> User()
        {
            List<AppUser> Users = await _um.Users.ToListAsync();
            return View(Users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _um.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return View("User");
            }

            var result = await _um.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("User");
            }
            return View("User");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _um.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _um.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Email = model.Email;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await _um.GeneratePasswordResetTokenAsync(user);
                    var result = await _um.ResetPasswordAsync(user, token, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                var updateResult = await _um.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("User");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                };
                var Result = await _um.CreateAsync(User, model.Password);

                if (Result.Succeeded)
                {
                    return RedirectToAction("User","Admin");
                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}

