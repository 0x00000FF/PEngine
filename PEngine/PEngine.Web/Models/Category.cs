using System.ComponentModel.DataAnnotations;


namespace PEngine.Web.Models;

public class Category
{
    [Key]
    public string Name { get; set; } = "";
    public long Count { get; set; }
    public int Order { get; set; }

}