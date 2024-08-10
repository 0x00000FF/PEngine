using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PEngine.Web.Models.Entities
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TypeId { get; set; }
        [ForeignKey("TypeId")]
        public DocumentType Type { get; set; }

        public Guid? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Member Author { get; set; }

        public Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Member Owner { get; set; }

        public Guid? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [Required]
        public int Permission { get; set; }

        [Required]
        public bool IsProtected { get; set; }

        public Guid? KeyChainId { get; set; }
        [ForeignKey("KeyChainId")]
        public KeyChain KeyChain { get; set; }

        [Required, StringLength(256)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
