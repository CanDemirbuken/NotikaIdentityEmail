using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LayoutComponents;

public class LayoutHeadViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}