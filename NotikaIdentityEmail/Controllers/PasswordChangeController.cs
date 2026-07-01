using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.ForgetPasswordModels;

namespace NotikaIdentityEmail.Controllers;

public class PasswordChangeController(UserManager<AppUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        var passwordResetLink = Url.Action("ResetPassword", "PasswordChange", new
        {
            userId = user.Id,
            token = passwordResetToken,
        }, Request.Scheme);

        MimeMessage message = new MimeMessage();
        MailboxAddress mailboxAddressFrom = new MailboxAddress("Notika Admin", "ycandemirbuken@gmail.com");

        message.From.Add(mailboxAddressFrom);

        MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
        message.To.Add(mailboxAddressTo);

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın: " + passwordResetLink;
        message.Body = bodyBuilder.ToMessageBody();

        message.Subject = "Notika Identity Şifre Sıfırlama";

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate("ycandemirbuken@gmail.com", "dmug zprd xxdz kfzv");
        smtpClient.Send(message);

        smtpClient.Disconnect(true);

        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string token)
    {
        var model = new ResetPasswordViewModel
        {
            UserId = userId,
            Token = token
        };

        return View(model);
    }

    [HttpPost]
    public async ValueTask<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token))
        {
            ViewBag.ErrorMessage = "Geçersiz kullanıcı veya token.";
            return View(model);
        }

        var user = await userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
            return View(model);
        }

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = "Şifre sıfırlanırken bir hata oluştu.";
            return View(model);
        }

        return RedirectToAction("UserLogin", "Login");
    }
}