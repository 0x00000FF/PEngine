using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models;

public class PostViewLog
{
    [Key]
    public long Id { get; set; }
    
    public Guid PostId { get; set; }
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = "";
    public string? Referer { get; set; }
    public string Country { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}