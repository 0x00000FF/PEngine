namespace PEngine.Models.Response;

public abstract class Response : IResponse
{
    public bool Status { get; set; }
    public string? Message { get; set; }
}