using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PEngine.Web.Models;

[Index(nameof(Order), IsUnique = true)]
public class Category
{
    [Key]
    public string Name { get; set; } = "";
    public long Count { get; set; }
    public int Order { get; set; }

}