using Api.Dtos.Dependent;
using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Database;

public class DatabaseConnection
{
    private readonly DatabaseConfig _config;

    public DatabaseConnection(DatabaseConfig config)
    {
        _config = config;
    }

    private const string EmployeeQuery = @"
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
WHERE e.Id == @EmployeeId
";
    
    public async Task<Employee?> GetEmployee(int id)
    {
        await using var connection = CreateConnection();
        var employeeRows = await connection.QueryAsync<EmployeeRow>(EmployeeQuery, new { EmployeeId = id });
        if (!employeeRows.Any())
        {
            return null;
        }
        var employee = ConvertEmployeeRowsToEmployees(employeeRows).Single();
        return employee;
    }

    private const string AllEmployeesQuery = @"
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

    public async Task<List<Employee>> GetAllEmployees()
    {
        await using var connection = CreateConnection();
        var employeeRows = await connection.QueryAsync<EmployeeRow>(AllEmployeesQuery);
        var employees = ConvertEmployeeRowsToEmployees(employeeRows);
        return employees;
    }

    private const string DependentQuery = @"
SELECT Id, FirstName, LastName, DateOfBirth, Relationship, EmployeeId
FROM Dependent
WHERE Id = @DependentId
";
    
    public async Task<Dependent?> GetDependent(int id)
    {
        await using var connection = CreateConnection();
        var dependent = await connection.QuerySingleOrDefaultAsync<Dependent>(DependentQuery, new { DependentId = id });
        return dependent;
    }

    private const string AllDependentsQuery = @"
SELECT Id, FirstName, LastName, DateOfBirth, Relationship, EmployeeId
FROM Dependent
";
    public async Task<List<Dependent>> GetAllDependents()
    {
        await using var connection = CreateConnection();
        var dependents = await connection.QueryAsync<Dependent>(AllDependentsQuery);
        return dependents.ToList();
    }

    private const string AddDependentCommand = @"

INSERT INTO Dependent (FirstName, LastName, DateOfBirth, Relationship, EmployeeId)
VALUES
    (@FirstName, @LastName, @DateOfBirth, @Relationship, @EmployeeId);
SELECT last_insert_rowid();
";
    public async Task<GetDependentDto> AddDependent(AddDependentDto addDependent)
    {
        await using var connection = CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(AddDependentCommand, addDependent);
        var dependent = await connection.QuerySingleAsync<GetDependentDto>(DependentQuery, new { DependentId = id });
        return dependent;
    }

    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_config.Name);
        return connection;
    }

    private static List<Employee> ConvertEmployeeRowsToEmployees(IEnumerable<EmployeeRow> employeeRows)
    {
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
}