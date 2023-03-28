namespace PEngine.Common.Misc;

public class ControlledObject : IControlledObject
{
    public Guid Owner { get; set; }
    public Guid Role { get; set; }
    public Permission Permission { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? ModifiedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}