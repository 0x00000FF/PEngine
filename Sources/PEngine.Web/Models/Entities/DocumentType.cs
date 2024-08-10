using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class DocumentType
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(32)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
