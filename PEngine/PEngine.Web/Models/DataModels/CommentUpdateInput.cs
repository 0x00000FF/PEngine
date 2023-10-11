namespace PEngine.Web.Models.DataModels;

public class CommentUpdateInput
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    public string Content { get; set; }

    public bool Screened { get; set; }
    public bool Encrypted { get; set; }
}