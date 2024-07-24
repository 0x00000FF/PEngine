namespace PEngine.Web.Models
{
    public class PostLike
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
