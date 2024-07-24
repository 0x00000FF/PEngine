
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models
{
    public class Post
    { 
        public Guid Id { get; set; } = Guid.NewGuid();

        public long No { get; set; }
        public Guid Version { get; set; } = Guid.NewGuid();
        
        public Guid WrittenBy { get; set; }

        public string? Category { get; set; }
        public Guid? Thumbnail { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime WrittenAt { get; set; } = DateTime.Now;
        public string? Tags { get; set; }

        public PostVisibility Visibility { get; set; }
        public string? ProtectedPassword { get; set; }
        
        public int Comments { get; set; }
        public int Hits { get; set; }
        public int Likes { get; set; }
    }

    public enum PostVisibility
    {
        Public, LinkOnly, Protected, Private
    }
}
