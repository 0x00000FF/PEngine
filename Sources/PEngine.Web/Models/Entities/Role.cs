using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(64)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
