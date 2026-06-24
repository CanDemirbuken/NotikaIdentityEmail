using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;

namespace NotikaIdentityEmail.Controllers;

public class LoginController(SignInManager<AppUser> signInManager) : Controller
{
    [HttpGet]
    public IActionResult UserLogin()
    {
        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> UserLogin(UserLoginViewModel model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
        return View();
    }
}