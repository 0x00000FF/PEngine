namespace PEngine.Web.Models.ViewModels;

public class DownloadResultVM
{
    public string? Name { get; set; }
    public string Type { get; set; } = "application/octet-stream";
    public long Length { get; set; }
    public Stream Stream { get; set; } = Stream.Null;
}