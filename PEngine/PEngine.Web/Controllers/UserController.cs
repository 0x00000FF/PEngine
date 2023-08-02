using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PEngine.Web.Helper;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;

namespace PEngine.Web.Controllers;

public class UserController : CommonControllerBase<UserController>
{
    private readonly BlogContext _context;
    public UserController(ILogger<UserController> logger,
        BlogContext context) : base(logger)
    {
        _context = context;
    }
    
    public IActionResult Login(string? returnUrl)
    {
        ViewData.Add("ReturnUrl", returnUrl ?? "/");
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user is null)
        {
            if (_context.Users.Any())
            {
                return View("UserNotFound");
            }
            else
            {
                return View("UserCreateFirst",
                    new UserCreateFirstVM { Username = username });
            }
        }

        if (user.Password != password.Password(user.PasswordSalt).ToBase64())
        {
            return View("UserFail");
        }

        var claimPrincipal = new ClaimsPrincipal();
        
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim("Name", user.Name));

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
            Id = Guid.NewGuid(),
            Name = name
        });

        return await _context.SaveChangesAsync() > 0 ? 
            Login(username, password) : BadRequest();
    }

    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await HttpContext.SignOutAsync();
        
        return Redirect(returnUrl ?? "/");
    }
}