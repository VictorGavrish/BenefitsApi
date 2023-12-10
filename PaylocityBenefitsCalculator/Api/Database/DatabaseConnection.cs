using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Data;

public class DatabaseConnection
{
    private readonly DatabaseConfig _config;

    public DatabaseConnection(DatabaseConfig config)
    {
        _config = config;
    }

    private class EmployeeRow
    {
        public int EmployeeId { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
        public decimal EmployeeSalary { get; set; }
        public DateTime EmployeeDateOfBirth { get; set; }
        public int? DependentId { get; set; }
        public string? DependentFirstName { get; set; }
        public string? DependentLastName { get; set; }
        public DateTime? DependentDateOfBirth { get; set; }
        public Relationship? DependentRelationship { get; set; }
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var connection = new SqliteConnection(_config.Name);
        var employeeRows = await connection.QueryAsync<EmployeeRow>(GetAllEmployeesQuery);
        var employees = employeeRows
            .GroupBy(er => er.EmployeeId)
            .Select(g =>
            {
                var first = g.First();
                var employee = new Employee
                {
                    Id = first.EmployeeId,
                    FirstName = first.EmployeeFirstName,
                    LastName = first.EmployeeLastName,
                    Salary = first.EmployeeSalary,
                    DateOfBirth = first.EmployeeDateOfBirth
                };
                var dependents = g.Where(g => g.DependentId.HasValue)
                    .Select(g => new Dependent
                    {
                        Id = g.DependentId.Value,
                        FirstName = g.DependentFirstName,
                        LastName = g.DependentLastName,
                        DateOfBirth = g.DependentDateOfBirth.Value,
                        Relationship = g.DependentRelationship.Value,
                        EmployeeId = first.EmployeeId,
                        Employee = employee
                    })
                    .ToList();
                employee.Dependents = dependents;
                return employee;
            })
            .ToList();
        return employees;
    }

    private const string GetAllEmployeesQuery = @"
SELECT e.Id as EmployeeId,
       e.FirstName as EmployeeFirstName,
       e.LastName as EmployeeLastName,
       e.Salary as EmployeeSalary,
       e.DateOfBirth as EmployeeDateOfBirth,
       d.Id as DependentId,
       d.FirstName as DependentFirstName,
       d.LastName as DependentLastName,
       d.DateOfBirth as DependentDateOfBirth,
       d.Relationship as DependentRelationship
FROM Employee e
LEFT JOIN Dependent d ON d.EmployeeId = e.Id
";
}