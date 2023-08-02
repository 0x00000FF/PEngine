using Microsoft.AspNetCore.Mvc;

namespace PEngine.Web.Controllers
{
    public class GuestbookController : CommonControllerBase<GuestbookController>
    {
        public GuestbookController(ILogger<GuestbookController> logger) : base(logger)
        {
        }
    }
}
