using System.ComponentModel.DataAnnotations;

namespace PEngine.Models.DataModels;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }

    public string Name { get; set; }

    public User(string username, string password, string name)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
        Name = name;
    }
}