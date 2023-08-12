using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEngine.Web.Helper;
using PEngine.Web.Models;

namespace PEngine.Web.Controllers;

public class ImageController : CommonControllerBase<ImageController>
{
    private readonly BlogContext _context;
    
    public ImageController(ILogger<ImageController> logger, BlogContext context) : base(logger)
    {
        _context = context;
    }
    
    [HttpGet("/[controller]/{id}")]
    public async Task<IActionResult> Show(Guid id)
    {
        var item = await _context.FileTags.FirstOrDefaultAsync(f => f.Id == id);

        if (item is null)
        {
            return StatusCode(404);
        }
        
        var stream = FileHelper.LoadAsStream(BasePath.UploadBase, item.Id.ToString());

        if (stream == Stream.Null || !item.Type.StartsWith("image"))
        {
            return StatusCode(410);
        }
        
        return File(stream, item.Type);
    }
}