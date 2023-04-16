namespace PEngine.Models.Response;

public interface IResponse
{
    public bool Status { get; set; }
    public string? Message { get; set; }
}