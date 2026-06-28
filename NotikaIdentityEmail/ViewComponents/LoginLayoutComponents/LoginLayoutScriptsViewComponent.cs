using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LoginLayoutComponents;

public class LoginLayoutScriptsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
