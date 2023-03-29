namespace PEngine.Exceptions;

public class PEngineException : Exception
{
    private Exception? _innerException;

    public PEngineException(Exception? innerException) : base(innerException?.Message)
    {
        _innerException = innerException;
    }
}