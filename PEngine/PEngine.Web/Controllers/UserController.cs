using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers;

public class UserController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(object credential)
    {
        return View();
    }

    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await HttpContext.SignOutAsync();
        return Redirect(returnUrl ?? "/");
    }
}