using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEngine.Web.Helper;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;

namespace PEngine.Web.Controllers;

public class FileController : CommonControllerBase<FileController>
{
    private readonly BlogContext _context;
    
    public FileController(ILogger<FileController> logger, BlogContext context) : base(logger)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadFile(List<IFormFile> files)
    {
        var succeeded = new List<UploadResultVM>();
        var failed = new List<UploadResultVM>();
        
        foreach (var file in files)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd");
            var entity = new FileTag {
                Id = Guid.NewGuid(),
                Name = file.FileName,
                Size = file.Length,
                Type = file.ContentType,
                UploadedBy = UserId!.Value
            };
            
            var entry = await _context.FileTags.AddAsync(entity);

            if (entry.State == EntityState.Added)
            {
                await using var stream = file.OpenReadStream();
                var path = $"{timestamp}/{entry.Entity.Id}";
                
                FileHelper.SaveFromStream(path, stream);
                
                succeeded.Add(null!);
            }
            else
            {
                entry.State = EntityState.Detached;
            }
        }

        return Json(new
        {
            succeeded, failed
        });
    }
    
}