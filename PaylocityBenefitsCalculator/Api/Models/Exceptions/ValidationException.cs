using Api.Dtos.Errors;

namespace Api.Models.Exceptions;

public sealed class ValidationException : ApiException
{
    public ValidationException(ValidationError error)
        : base("There were validation errors")
    {
        ErrorData = error;
    }

    public override object ErrorData { get; protected set; }
}