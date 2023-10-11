using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models
{
    public class Post
    {
        [Key]
        public long Id { get; set; }
        public Guid WrittenBy { get; set; }
        public Guid? Thumbnail { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime WrittenAt { get; set; } = DateTime.Now;
        
        [NotMapped]
        [Obsolete("No longer be used, migrated to PostViewCount.Count")]
        public int Hits { get; set; }
    }
}
