namespace Shared.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string message) : base(message)
    {
        
    }

    public abstract string Message { get;}
}