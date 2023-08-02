using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? WrittenBy { get; set; }
        public long Post { get; set; }
        public string? WriterName { get; set; }
        public string? Password { get; set; }
        public string? Content { get; set; }
        public DateTime WrittenAt { get; set; } = DateTime.Now;
    }
}
