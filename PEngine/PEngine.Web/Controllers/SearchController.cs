using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    [Route("/[controller]/{keyword}")]
    public class SearchController : Controller
    {
        [HttpGet]
        public IActionResult Result(string keyword)
        {
            return NotFound();
        }
    }
}
