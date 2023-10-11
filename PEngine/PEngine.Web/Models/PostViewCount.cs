using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models;

public class PostCount
{
    [Key]
    public Guid PostId { get; set; }

    public long LikeCount { get; set; }
    public long ViewCount { get; set; }
}