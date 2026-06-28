using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers;

public class ActivationController(EmailDbContext context) : Controller
{
    [HttpGet]
    public IActionResult UserActivation()
    {
        var email = TempData["EmailKey"];
        TempData["EmailActivation"] = email;

        ViewBag.email = email;

        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> UserActivation(int userCodeParameter)
    {
        string email = TempData.Peek("EmailActivation").ToString();
        var user = context.Users.Where(u => u.Email == email).FirstOrDefault();
        var code = user.ActivationCode;

        if (userCodeParameter == code)
        {
            user.EmailConfirmed = true;
            context.SaveChanges();

            return RedirectToAction("UserLogin", "Login");
        }

        return View();
    }
}