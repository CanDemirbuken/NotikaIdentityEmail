using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.RegisterLayoutComponents;

public class RegisterLayoutHeadViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}