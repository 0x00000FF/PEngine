using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class File
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UploaderId { get; set; }
        [ForeignKey("UploaderId")]
        public Member Uploader { get; set; }

        public string Name { get; set; }
        public long? Size { get; set; }
        public string Type { get; set; }

        public Guid StorageId { get; set; }
        [ForeignKey("StorageId")]
        public FileStorage Storage { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
