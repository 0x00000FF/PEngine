using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;

namespace PEngine.Web.Controllers
{
    public class HomeController : CommonControllerBase<HomeController>
    {
        private readonly BlogContext _context;
        
        public HomeController(ILogger<HomeController> logger, BlogContext context) : base(logger)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM model = new();

            model.Latest = _context.Posts.OrderByDescending(p => p.Id)
                .Take(4).ToList();

            model.Categories = new Dictionary<string, List<Post>>();
            
            return View(model);
        }

        [HttpGet("/Misc/Mdd")]
        public IActionResult Mdd()
        {
            return View();
        }
    }
}