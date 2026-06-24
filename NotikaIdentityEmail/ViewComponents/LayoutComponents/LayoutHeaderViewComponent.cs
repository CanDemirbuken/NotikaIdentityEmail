using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LayoutComponents;

public class LayoutHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
