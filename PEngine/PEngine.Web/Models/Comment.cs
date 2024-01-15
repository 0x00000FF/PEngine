using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ReplyOf { get; set; }
        
        public Guid? WrittenBy { get; set; }
        
        public long Post { get; set; }
        public string? WriterName { get; set; }
        public long WriterIp { get; set; }
        public string? WriterUserAgent { get; set; }
        public string? Password { get; set; }
        public string? Content { get; set; }
        public long WrittenAt { get; set; } = (long) DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public bool IsFixed { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHidden { get; set; }
    }
}
