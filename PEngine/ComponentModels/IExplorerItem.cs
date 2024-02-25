namespace PEngine.ComponentModels;

public interface IExplorerItem
{
    public bool IsSelected { get; set; }
    public object? Tag { get; set; }
}