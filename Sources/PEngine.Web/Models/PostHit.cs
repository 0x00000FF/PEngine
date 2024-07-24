namespace PEngine.Web.Models
{
    public class PostHit
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
