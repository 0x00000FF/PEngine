using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using PEngine.Web.Helper;
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

        [HttpGet("/Rss")]
        [ResponseCache(Duration = 600)]
        public async Task<IActionResult> Rss()
        {
            var posts = _context.Posts.OrderByDescending(p => p.WrittenAt)
                .Take(10)
                .ToList()
                .Select(p =>
                {
                    p.Content = ContentHelper.GetPlaintext(p.Content ?? "", 100);
                    return p;
                });

            var feed = new SyndicationFeed(
                "Patche's Blog", "The RSS Feed from Patche's Blog",
                new Uri("https://patche.me"), "https://patche.me/Rss", DateTime.Now);

            var items = new List<SyndicationItem>();
            
            foreach (var post in posts)
            {
                var url = Url.Action("View", "Post", new { id = post.Id });
                var title = post.Title;
                var desc = post.Content;
                
                items.Add(new (title, desc, new Uri(url!)));
            }

            feed.Items = items;

            await using var stream = new MemoryStream();
            var formatter = new Rss20FeedFormatter(feed);

            await using var writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Async = true,
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            });
            
            formatter.WriteTo(writer);
            await writer.FlushAsync();

            return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
        }
    }
}