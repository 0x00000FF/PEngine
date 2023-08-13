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
    
    public async Task<IActionResult> Download(Guid id)
    {
        var tag = await _context.FileTags.FirstOrDefaultAsync(f => f.Id == id);

        if (tag is null)
        {
            return StatusCode(404);
        }

        var file = FileHelper.LoadAsStream(BasePath.UploadBase, $"{tag.Id}");

        if (file == Stream.Null)
        {
            return StatusCode(410);
        }

        return File(file, tag.Type, tag.Name);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadSingle(IFormFile file)
    {
        return await Upload(new List<IFormFile>() { file });
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Upload(List<IFormFile> files)
    {
        var succeeded = new List<UploadResultVM>();
        var failed = new List<UploadResultVM>();
        
        foreach (var file in files)
        {
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
                var path = $"{entry.Entity.Id}";
                
                FileHelper.SaveFromStream(BasePath.UploadBase, path, stream);
                
                succeeded.Add(new ()
                {
                    Name = file.FileName,
                    Size = file.Length,
                    Type = file.ContentType,
                    Location = $"/File/Download/{path}"
                });
            }
            else
            {
                entry.State = EntityState.Detached;
            }

            await _context.SaveChangesAsync();
        }

        return succeeded.Count == 1 ? Json(succeeded[0])
            : Json(new { succeeded, failed });
    }
    
}