using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid VersionId { get; set; }
        [ForeignKey("VersionId")]
        public PostVersion Version { get; set; }

        [Required, StringLength(32)]
        public string Visibility { get; set; }

        public string Permalink { get; set; }

        public string Password { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
