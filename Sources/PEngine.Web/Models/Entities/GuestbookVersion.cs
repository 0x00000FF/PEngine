using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class GuestbookVersion
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}
