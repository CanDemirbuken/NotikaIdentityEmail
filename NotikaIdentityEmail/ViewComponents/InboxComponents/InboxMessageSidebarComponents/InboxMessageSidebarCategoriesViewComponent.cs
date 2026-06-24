using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.ViewComponents.InboxComponents.InboxMessageSidebarComponents;

public class InboxMessageSidebarCategoriesViewComponent(EmailDbContext context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await context.Categories.ToListAsync();
        return View(categories);
    }
}