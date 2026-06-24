using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;

namespace NotikaIdentityEmail.Controllers;

public class MessageController(EmailDbContext context) : Controller
{
    public async ValueTask<IActionResult> Inbox()
    {
        ViewBag.Title = "Gelen Kutusu";
        ViewBag.Description = "Gelen kutunuzu bu sayfada görüntüleyebilirsiniz.";

        var messages = await context.Messages.Where(x => x.ReceiverEmail == "ycandemirbuken@gmail.com").Include(y => y.Category).ToListAsync();
        return View(messages);
    }

    public async ValueTask<IActionResult> Sendbox()
    {
        ViewBag.Title = "Gönderilenler";
        ViewBag.Description = "Gönderilen mesajları bu sayfada görüntüleyebilirsiniz.";

        var messages = await context.Messages.Where(x => x.SenderEmail == "ycandemirbuken@gmail.com").Include(y => y.Category).ToListAsync();
        return View(messages);
    }

    public async ValueTask<IActionResult> MessageDetail()
    {
        ViewBag.Title = "Mesaj Detayları";
        ViewBag.Description = "Mesaj detaylarını bu sayfada görüntüleyebilirsiniz.";

        var message = await context.Messages.Where(x => x.Id == 1).FirstOrDefaultAsync();
        return View(message);
    }

    [HttpGet]
    public async ValueTask<IActionResult> ComposeMessage()
    {
        ViewBag.Title = "Mesaj Gönder";
        ViewBag.Description = "Mesaj gönderme işlemlerini bu sayfadan gerçekleştirebilirsiniz.";

        var categories = await context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            })
            .ToListAsync();

        ViewBag.Categories = categories;

        return View();
    }
}