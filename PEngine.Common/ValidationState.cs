namespace PEngine.Common;

public class ValidationState
{
    public bool IsSuccess => string.IsNullOrEmpty(Message);
    public string? Message { get; }

    public ValidationState()
    {
        
    }

    public ValidationState(string message)
    {
        Message = message;
    }
    
    public static ValidationState Success => new ValidationState();
    public static ValidationState Failed(string message) => new ValidationState(message);
}