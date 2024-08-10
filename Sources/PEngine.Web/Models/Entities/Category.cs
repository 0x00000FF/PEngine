using System;
using System.ComponentModel.DataAnnotations;

namespace PEngine.Web.Models.Entities
{
    public class Category
    {
        [Key, StringLength(32)]
        public string Name { get; set; }

        public int? Count { get; set; } = 0;

        [Required]
        public int Ord { get; set; }
    }
}
