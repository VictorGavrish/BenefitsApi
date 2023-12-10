namespace Api.Models.Exceptions;

public abstract class ApiException : Exception
{
    public abstract string Message { get; }
    public abstract object Data { get; protected set; }
}