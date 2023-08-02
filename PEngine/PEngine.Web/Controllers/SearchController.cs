using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    [Route("/[controller]/{keyword}")]
    public class SearchController : CommonControllerBase<ControllerBase>
    {
        public SearchController(ILogger<SearchController> logger) : base(logger)
        {
            
        }
        
        [HttpGet]
        public IActionResult Result(string keyword)
        {
            return NotFound();
        }
    }
}
