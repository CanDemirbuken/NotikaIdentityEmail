using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.InboxComponents.InboxMessageSidebarComponents;

public class InboxMessageSidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}