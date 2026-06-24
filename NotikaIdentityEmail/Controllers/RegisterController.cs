using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;

namespace NotikaIdentityEmail.Controllers;

public class RegisterController(UserManager<AppUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateUser(RegisterUserViewModel model)
    {
        AppUser appUser = new AppUser()
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            UserName = model.Username
        };

        var result = await userManager.CreateAsync(appUser, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return RedirectToAction("UserLogin", "Login");
    }
}