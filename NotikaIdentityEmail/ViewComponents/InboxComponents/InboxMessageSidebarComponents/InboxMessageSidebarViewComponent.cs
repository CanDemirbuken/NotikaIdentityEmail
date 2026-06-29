using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;

namespace NotikaIdentityEmail.ViewComponents.InboxComponents.InboxMessageSidebarComponents;

public class InboxMessageSidebarViewComponent(EmailDbContext context, UserManager<AppUser> userManager) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);

        ViewBag.SendMessageCount = await context.Messages.Where(x => x.SenderEmail == user.Email).CountAsync();
        ViewBag.ReceiveMessageCount = await context.Messages.Where(x => x.ReceiverEmail == user.Email).CountAsync();

        return View();
    }
}