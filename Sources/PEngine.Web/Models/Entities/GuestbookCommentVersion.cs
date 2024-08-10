using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class GuestbookCommentVersion
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}
