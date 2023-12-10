using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Errors;
using Api.Logic;
using Api.Models.Exceptions;

namespace Api.Services;

public class DependentService
{
    private readonly DatabaseConnection _connection;
    private readonly ValidationService _validation;

    public DependentService(DatabaseConnection connection, ValidationService validation)
    {
        _connection = connection;
        _validation = validation;
    }
    
    public async Task<GetDependentDto> AddDependent(AddDependentDto addDependent)
    {
        var validationMessages = await _validation.ValidateNewDependent(addDependent);
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

    public async Task<GetDependentDto?> GetDependent(int id)
    {
        var dependent = await _connection.GetDependent(id);
        return dependent;
    }

    public async Task<List<GetDependentDto>> GetAllDependents()
    {
        var dependents = await _connection.GetAllDependents();
        return dependents;
    }
}