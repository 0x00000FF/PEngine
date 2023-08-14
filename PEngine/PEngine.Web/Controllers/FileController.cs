using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEngine.Web.Helper;
using PEngine.Web.Models;
using PEngine.Web.Models.ViewModels;
using SixLabors.ImageSharp.Formats.Png;

namespace PEngine.Web.Controllers;

public class FileController : CommonControllerBase<FileController>
{
    private readonly BlogContext _context;
    
    public FileController(ILogger<FileController> logger, BlogContext context) : base(logger)
    {
        _context = context;
    }

    private async Task<DownloadResultVM> GetFile(Guid id, string typeFilter)
    {
        var tag = await _context.FileTags.FirstOrDefaultAsync(f => f.Id == id &&
                                                                   f.Type.StartsWith(typeFilter));
        var result = new DownloadResultVM();
        
        if (tag is null)
        {
            Logger.Log(LogLevel.Warning, "Requested type {0}, but not found for filtered", typeFilter);
            return result;
        }
        
        var stream  = FileHelper.LoadAsStream(BasePath.UploadBase, $"{id}");
        
        if (stream == Stream.Null)
        {
            Logger.Log(LogLevel.Warning, "Requested type {0}, but file does not exist.", typeFilter);
            return result;
        }

        result.Name = tag.Name;
        result.Type = tag.Type;
        result.Length = tag.Size;
        result.Stream = stream;

        return result;
    }

    private async Task<Stream> TryCreateThumbnail(Guid id)
    {
        return await TryCreateThumbnail(id, 300, 300);
    }
    private async Task<Stream> TryCreateThumbnail(Guid id, int width, int height)
    {
        var req = await GetFile(id, "image/");

        if (req.Stream == Stream.Null)
        {
            return Stream.Null;
        }

        var sourceImage = await SixLabors.ImageSharp.Image.LoadAsync(req.Stream);
        var destStream = new MemoryStream();
        
        sourceImage.Mutate(ctx =>
        {
            ctx.Resize(width, height);
        });
        
        await sourceImage.SaveAsync(destStream, new PngEncoder());
        destStream.Position = 0;

        FileHelper.SaveFromStream(BasePath.ThumbnailsBase, $"{id}", destStream);
        destStream.Position = 0;
        
        return destStream;
    }
    
    public async Task<IActionResult> Download(Guid id)
    {
        var req = await GetFile(id, "");

        if (req.Stream == Stream.Null)
        {
            return NotFound();
        }
        
        return File(req.Stream, "application/octet-stream", req.Name);
    }

    public async Task<IActionResult> Image(Guid id)
    {
        var req = await GetFile(id, "image/");
        
        if (req.Stream == Stream.Null)
        {
            return Redirect("/img/NotFound.png");
        }
        
        return File(req.Stream, req.Type, req.Name);
    }
    
    public async Task<IActionResult> Thumbnail(Guid id)
    {
        var stream = FileHelper.LoadAsStream(BasePath.ThumbnailsBase, $"{id}");

        if (stream == Stream.Null)
        {
            stream = await TryCreateThumbnail(id);
        }

        return File(stream, "image/png");
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