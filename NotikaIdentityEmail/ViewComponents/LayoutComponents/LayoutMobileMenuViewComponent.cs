using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LayoutComponents;

public class LayoutMobileMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
