using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LayoutComponents;

public class LayoutBreadCombViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}