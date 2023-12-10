using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Errors;
using Api.Models.Exceptions;

namespace Api.Logic;

public class Command
{
    private readonly Validity _validity;
    private readonly DatabaseConnection _connection;

    public Command(Validity validity, DatabaseConnection connection)
    {
        _validity = validity;
        _connection = connection;
    }
    
    public async Task<GetDependentDto> AddDependent(AddDependentDto addDependent)
    {
        var validationMessages = await _validity.ValidateNewDependent(addDependent);
        if (validationMessages.Any())
        {
            throw new ValidationException(new ValidationError
            {
                Errors = validationMessages
            });
        }

        var getDependent = await _connection.AddDependent(addDependent);
        return getDependent;
    }
}