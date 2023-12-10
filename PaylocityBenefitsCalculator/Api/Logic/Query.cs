using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Logic;

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

    public async Task<GetEmployeeDto?> Employee(int id)
    {
        var employee = await _connection.GetEmployee(id);
        if (employee == null)
        {
            return null;
        }

        var dto = ConvertEmployeeToDto(employee);
        return dto;
    }

    public async Task<List<GetDependentDto>> AllDependents()
    {
        var dependents = await _connection.GetAllDependents();
        var dtos = dependents.Select(ConvertDependentToDto).ToList();
        return dtos;
    }

    public async Task<GetDependentDto?> Dependent(int id)
    {
        var dependent = await _connection.GetDependent(id);
        if (dependent == null)
        {
            return null;
        }

        var dto = ConvertDependentToDto(dependent);
        return dto;
    }

    private static GetEmployeeDto ConvertEmployeeToDto(Employee employee)
    {
        var dependents = employee.Dependents.Select(ConvertDependentToDto).OrderBy(d => d.Id).ToList();
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