namespace PEngine.Common.Misc;

public interface IControlledObject
{
    public Guid Owner { get; set; }
    public Guid Role { get; set; }
    public Permission Permission { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}