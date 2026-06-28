using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.LoginLayoutComponents;

public class LoginLayoutHeadViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}