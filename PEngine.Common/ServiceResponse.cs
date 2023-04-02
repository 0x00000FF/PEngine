namespace PEngine.Common;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public T? Payload { get; set; }
}