namespace PEngine.Web.Models.DataModels;

public class CommentQueryResult
{
    public Comment Comment { get; set; } = null!;
    public bool Deleted { get; set; }
    public bool Screened { get; set; }
    public bool Encrypted { get; set; }
    public long Replies { get; set; }
}