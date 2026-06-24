using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.Controllers;

public class ProfileController : Controller
{
    public IActionResult EditProfile()
    {
        ViewBag.Title = "Profil Yönetimi";
        ViewBag.Description = "Profil bilgilerinizi bu sayfa üzerinden yönetebilirsiniz.";
        return View();
    }
}