using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LayoutComponents;

public class LayoutMainMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}