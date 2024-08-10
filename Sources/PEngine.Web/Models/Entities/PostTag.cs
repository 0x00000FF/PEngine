using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class PostTag
    {
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [Required, StringLength(32)]
        public string Tag { get; set; }

        [Index("IX_Post_Tag", IsUnique = true)]
        public string UniqueIndex => $"{PostId}_{Tag}";
    }
}
