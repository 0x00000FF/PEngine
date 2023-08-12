using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PEngine.Web.Models;

public class Category
{
[Key]
    public string Name { get; set; } = "";
    public long Count { get; set; }
}