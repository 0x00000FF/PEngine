using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View();
    }
}