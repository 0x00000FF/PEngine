using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PEngine.Web.Models;

[Index(nameof(CommentId), nameof(PostId), IsUnique = true)]
public class CommentMeta
{
    [Key]
    public Guid CommentId { get; set; }
    public long PostId { get; set; }
    
    public string? Path { get; set; }

    public long ChildrenCount { get; set; }

    public bool Deleted { get; set; }
    public bool Screened { get; set; }
    public bool Encrypted { get; set; }
}