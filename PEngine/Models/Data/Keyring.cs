namespace PEngine.Models.Data;

public class Keyring
{
    public Guid Id { get; set; }
    public string? Salt { get; set; }
    public string Algorithm { get; set; }
    public string Key { get; set; }
    public string? Vector { get; set; }
}