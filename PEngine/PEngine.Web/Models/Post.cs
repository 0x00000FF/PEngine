using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models
{
    public class Post
    {
        [Key]
        public long Id { get; set; }
        public Guid WrittenBy { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime WrittenAt { get; set; } = DateTime.Now;
        public int Hits { get; set; }
    }
}
