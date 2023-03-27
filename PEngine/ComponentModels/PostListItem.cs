using PEngine.Attributes;

namespace PEngine.ComponentModels;

public class PostListItem : IExplorerItem
{
    public bool IsSelected { get; set; }
    
    [ExplorerViewColumn(Name = "Title")]
    public string? Title { get; set; }
    
    [ExplorerViewColumn(Name = "Category")]
    public string? Category { get; set; }
    
    [ExplorerViewColumn(Name = "Date Created")]
    public DateTimeOffset DateCreated { get; set; }
    
    [ExplorerViewColumn(Name = "Date Modified")]
    public DateTimeOffset DateModified { get; set; }
    
    [ExplorerViewColumn(Name = "Author")]
    public string? Author { get; set; }
    
    [ExplorerViewColumn(Name = "Size")]
    public string? Size { get; set; }
}