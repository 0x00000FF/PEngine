namespace PEngine.Models.Data;

public class Post
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public bool Encrypted { get; set; }
    public Guid? Keyring { get; set; }
}