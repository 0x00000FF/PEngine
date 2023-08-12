using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models
{
    public class Guestbook
    {
        [Key]
        public long Id { get; set; }
        public Guid? WrittenBy { get; set; }
        public long ReplyTo { get; set; }
        public string? WriterName { get; set; }
        public string? WriterIp { get; set; }
        public string? Password { get; set; }
        public string? Content { get; set; }
        public DateTime WrittenAt { get; set; } = DateTime.Now;
    }
}
