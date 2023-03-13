using System.ComponentModel.DataAnnotations;

namespace PEngine.Models.DataModels;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }

    public Guid? Profile { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }

    public string? SNSHandles { get; set; }
}