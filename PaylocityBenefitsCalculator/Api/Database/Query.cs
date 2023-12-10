using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Data;

public class Query
{
    private readonly DatabaseConnection _connection;

    public Query(DatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<GetEmployeeDto>> AllEmployees()
    {
        var employees = await _connection.GetAllEmployees();
        var dtos = employees.Select(ConvertEmployeeToDto).ToList();
        return dtos;
    }

    private static GetEmployeeDto ConvertEmployeeToDto(Employee employee)
    {
        var dependents = employee.Dependents.Select(ConvertDependentToDto).ToList();
        var dto = new GetEmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = dependents,
        };
        return dto;
    }

    private static GetDependentDto ConvertDependentToDto(Dependent dependent)
    {
        var dto = new GetDependentDto
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            Relationship = dependent.Relationship
        };
        return dto;
    }
}