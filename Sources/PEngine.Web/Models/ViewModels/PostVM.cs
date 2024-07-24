namespace PEngine.Web.Models.ViewModels;

public class PostVM
{
    public long Id { get; set; }
    public string? WriterName { get; set; }
    public Guid? Thumbnail { get; set; }

    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? Content { get; set; }
    
    public DateTime WrittenAt { get; set; }
    
    public long Hits { get; set; }
}