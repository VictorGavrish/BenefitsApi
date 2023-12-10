namespace Api.Models.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(string message)
        : base(message)
    {
    }
    public abstract object ErrorData { get; protected set; }
}