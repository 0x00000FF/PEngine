using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PEngine.Web.Helper;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;

namespace PEngine.Web.Controllers;

public class MemberController : CommonControllerBase<MemberController>
{
    public MemberController(ILogger<MemberController> logger) : base(logger)
    {

    }
    
    public IActionResult Login(string? returnUrl)
    {
        if (!_context.Users.Any())
        {
            return View("UserCreateFirst", new UserCreateFirstVM { });
        }

        ViewData.Add("ReturnUrl", returnUrl ?? "/");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password, string? ipsec)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user is null)
        {
            return View("UserNotFound");
        }

        if (user.Password != password.Password(user.PasswordSalt!).ToBase64())
        {
            // TODO: make option for failing password matches:
            // return View("UserNotFound");

            return View("UserFail");
        }

        var claimPrincipal = new ClaimsPrincipal();
        
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim("Name", user.Name!));
        
        // IP-Security
        if (ipsec == "y")
        {
            claimsIdentity.AddClaim(new Claim("IPSec", "y"));

            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            
            if (remoteIp is null)
            {
                return View("UserFailIPAddr");
            }

            claimsIdentity.AddClaim(new Claim("IPAddress", remoteIp.ToString()));
        }

        claimPrincipal.AddIdentity(claimsIdentity);

        return SignIn(claimPrincipal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFirst(string username, string password, string name)
    {
        if (_context.Users.Any())
        {
            return Forbid();
        }
        
        var newSalt = CryptoHelper.Random(32).ToBase64();
        _context.Users.Add(new User()
        {
            Username = username,
            Password = password.Password(newSalt).ToBase64(),
            PasswordSalt = newSalt,
            Id = default,
            Name = name,
            Role = UserRole.Root
        });

        return await _context.SaveChangesAsync() > 0 ? 
            RedirectToAction("Login") : BadRequest();
    }

    public async Task<IActionResult> Logout(string? returnUrl)
    {
        if (IsAuthenticated)
        {
            await HttpContext.SignOutAsync();
        }

        return Redirect(returnUrl ?? "/");
    }
}