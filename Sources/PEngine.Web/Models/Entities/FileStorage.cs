using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class FileStorage
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(32)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Description { get; set; } = "";

        [Required]
        public string Path { get; set; }

        [StringLength(32)]
        public string Provider { get; set; }
    }
}
