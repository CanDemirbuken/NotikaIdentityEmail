using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;

[Authorize]
public class ProfileController(UserManager<AppUser> userManager) : Controller
{
    [HttpGet]
    public async ValueTask<IActionResult> EditProfile()
    {
        ViewBag.Title = "Profil Yönetimi";
        ViewBag.Description = "Profil bilgilerinizi bu sayfa üzerinden yönetebilirsiniz.";

        var user = await userManager.FindByNameAsync(User.Identity.Name);

        UserEditViewModel model = new UserEditViewModel();
        model.Name = user.Name;
        model.Surname = user.Surname;
        model.PhoneNumber = user.PhoneNumber;
        model.ImageUrl = user.ImageUrl;
        model.City = user.City;
        model.Username = user.UserName;
        model.Email = user.Email;

        return View(model);
    }

    [HttpPost]
    public async ValueTask<IActionResult> EditProfile(UserEditViewModel model)
    {
        if (model.Password == model.PasswordConfirm)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.ImageUrl = model.ImageUrl;
            user.UserName = model.Username;
            user.Email = model.Email;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

            await userManager.UpdateAsync(user);
        }

        ModelState.AddModelError("", "Şifrelerin eşleştiğinden emin olun.");
        return View();
    }
}