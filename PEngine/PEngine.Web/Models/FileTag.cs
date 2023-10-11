using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models;

public class FileTag
{
    [Key]
    public Guid Id { get; set; }
    public Guid UploadedBy { get; set; }
    public string? Name { get; set; }
    public long Size { get; set; }
    public string Type { get; set; } = null!;
    public DateTime WrittenAt { get; set; } = DateTime.Now;
}