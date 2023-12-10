using Api.Dtos.Errors;

namespace Api.Models.Exceptions;

public sealed class ValidationException : ApiException
{
    public ValidationException(ValidationError error)
    {
        Data = error;
    }
    public override string Message => "There were validation errors";
    public override object Data { get; protected set; }
}