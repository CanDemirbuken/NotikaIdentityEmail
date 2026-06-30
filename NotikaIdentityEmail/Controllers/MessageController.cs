using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.MessageViewModels;

namespace NotikaIdentityEmail.Controllers;

public class MessageController(EmailDbContext context, UserManager<AppUser> userManager) : Controller
{
    public async ValueTask<IActionResult> Inbox()
    {
        ViewBag.Title = "Gelen Kutusu";
        ViewBag.Description = "Gelen kutunuzu bu sayfada görüntüleyebilirsiniz.";

        var user = await userManager.FindByNameAsync(User.Identity.Name);
        var messages = (from m in context.Messages
                        join u in context.Users
                            on m.SenderEmail equals u.Email into userGroup
                        from sender in userGroup.DefaultIfEmpty()
                        join c in context.Categories
                            on m.CategoryId equals c.Id into categoryGroup
                        from category in categoryGroup.DefaultIfEmpty()
                        where m.ReceiverEmail == user.Email
                        select new MessageWithSenderInfoViewModel
                        {
                            Id = m.Id,
                            Detail = m.Detail,
                            Subject = m.Subject,
                            SendDate = m.SendDate,
                            SenderEmail = m.SenderEmail,
                            SenderName = sender != null ? sender.Name : "Bilinmeyen",
                            SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                            CategoryName = category != null ? category.Name : "Kategori Yok"
                        }).OrderByDescending(x => x.SendDate).ToList();

        //var messages = await context.Messages.Where(x => x.ReceiverEmail == user.Email).Include(y => y.Category).ToListAsync();
        return View(messages);
    }

    public async ValueTask<IActionResult> Sendbox()
    {
        ViewBag.Title = "Gönderilenler";
        ViewBag.Description = "Gönderilen mesajları bu sayfada görüntüleyebilirsiniz.";

        var user = await userManager.FindByNameAsync(User.Identity.Name);
        var messages = (from m in context.Messages
                        join u in context.Users
                            on m.ReceiverEmail equals u.Email into userGroup
                        from receiver in userGroup.DefaultIfEmpty()
                        join c in context.Categories
                            on m.CategoryId equals c.Id into categoryGroup
                        from category in categoryGroup.DefaultIfEmpty()
                        where m.SenderEmail == user.Email
                        select new MessageWithReceiverInfoViewModel
                        {
                            Id = m.Id,
                            Detail = m.Detail,
                            Subject = m.Subject,
                            SendDate = m.SendDate,
                            ReceiverEmail = m.ReceiverEmail,
                            ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                            ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                            CategoryName = category != null ? category.Name : "Kategori Yok"
                        }).OrderByDescending(x => x.SendDate).ToList();

        return View(messages);
    }

    public async ValueTask<IActionResult> MessageDetail(int id)
    {
        ViewBag.Title = "Mesaj Detayları";
        ViewBag.Description = "Mesaj detaylarını bu sayfada görüntüleyebilirsiniz.";

        var message = await context.Messages.Where(x => x.Id == id).FirstOrDefaultAsync();
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

    [HttpPost]
    public async ValueTask<IActionResult> ComposeMessage(Message message)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);

        message.SenderEmail = user.Email;
        message.SendDate = DateTime.Now;
        message.IsRead = false;

        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync();

        return RedirectToAction("Sendbox");
    }
}