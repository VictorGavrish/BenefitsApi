using Api.Database;
using Api.Dtos.Employee;
using Api.Logic;

namespace Api.Services;

public class EmployeeService
{
    private readonly DatabaseConnection _connection;
    private readonly PaycheckCalculator _paycheck;

    public EmployeeService(DatabaseConnection connection, PaycheckCalculator paycheck)
    {
        _connection = connection;
        _paycheck = paycheck;
    }
    public async Task<GetPaycheckDto?> GetPaycheck(int employeeId)
    {
        var employee = await _connection.GetEmployee(employeeId);
        if (employee == null)
        {
            return null;
        }
        var paycheck = _paycheck.GetPaycheck(employee, DateTime.Now);
        return paycheck;
    }

    public async Task<GetEmployeeDto?> GetEmployee(int id)
    {
        var employee = await _connection.GetEmployee(id);
        return employee;
    }

    public async Task<List<GetEmployeeDto>> GetAllEmployees()
    {
        var employees = await _connection.GetAllEmployees();
        return employees;
    }
}