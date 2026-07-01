using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;

public class LoginController(EmailDbContext context, SignInManager<AppUser> signInManager) : Controller
{
    [HttpGet]
    public IActionResult UserLogin()
    {
        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> UserLogin(UserLoginViewModel model)
    {
        var user = await context.Users.Where(u => u.UserName == model.Username).FirstOrDefaultAsync();

        if(user == null)
        {
            ModelState.AddModelError("", "Kullanıcı bulunamadı.");
            return View(model);
        }

        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError("", "Kullanıcı aktivasyon süreci tamamlanmadı.");
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
        if (result.Succeeded)
            return RedirectToAction("EditProfile", "Profile");

        ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
        return View(model);
    }
}