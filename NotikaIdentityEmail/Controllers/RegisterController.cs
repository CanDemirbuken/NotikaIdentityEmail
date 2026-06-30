using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;

public class RegisterController(UserManager<AppUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateUser(UserRegisterViewModel model)
    {
        Random rnd = new Random();
        int code = rnd.Next(100000, 1000000);

        AppUser appUser = new AppUser()
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            UserName = model.Username,
            ActivationCode = code
        };

        var result = await userManager.CreateAsync(appUser, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View();
            }
        }

        MimeMessage mimeMessage = new MimeMessage();
        MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "ycandemirbuken@gmail.com");

        mimeMessage.From.Add(mailboxAddressFrom);

        MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
        mimeMessage.To.Add(mailboxAddressTo);

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Hesabınızı doğrulamak için gerekli olan aktivasyon kodu: " + code;
        mimeMessage.Body = bodyBuilder.ToMessageBody();

        mimeMessage.Subject = "Notika Identity Aktivasyon Kodu";

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate("ycandemirbuken@gmail.com", "dmug zprd xxdz kfzv");
        smtpClient.Send(mimeMessage);

        smtpClient.Disconnect(true);

        TempData["EmailKey"] = model.Email;

        return RedirectToAction("UserActivation", "Activation");
    }
}